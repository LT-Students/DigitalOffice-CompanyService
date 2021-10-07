using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using LT.DigitalOffice.CompanyService.Broker.Consumers;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using LT.DigitalOffice.Kernel.Middlewares.Token;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Serilog;
using System.Text.RegularExpressions;

namespace LT.DigitalOffice.CompanyService
{
  public class Startup : BaseApiInfo
  {
    public const string CorsPolicyName = "LtDoCorsPolicy";

    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly BaseServiceInfoConfig _serviceInfoConfig;

    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      _rabbitMqConfig = Configuration
        .GetSection(BaseRabbitMqConfig.SectionName)
        .Get<RabbitMqConfig>();

      _serviceInfoConfig = Configuration
        .GetSection(BaseServiceInfoConfig.SectionName)
        .Get<BaseServiceInfoConfig>();

      Version = "1.6.2.2";
      Description = "CompanyService is an API that intended to work with positions and departments.";
      StartTime = DateTime.UtcNow;
      ApiName = $"LT Digital Office - {_serviceInfoConfig.Name}";
    }

    #region public methods

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(
          CorsPolicyName,
          builder =>
          {
            builder
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
          });
      });

      if (int.TryParse(Environment.GetEnvironmentVariable("RedisCacheLiveInMinutes"), out int redisCacheLifeTime))
      {
        services.Configure<RedisConfig>(options =>
        {
          options.CacheLiveInMinutes = redisCacheLifeTime;
        });
      }
      else
      {
        services.Configure<RedisConfig>(Configuration.GetSection(RedisConfig.SectionName));
      }

      services.Configure<TokenConfiguration>(Configuration.GetSection("CheckTokenMiddleware"));
      services.Configure<BaseRabbitMqConfig>(Configuration.GetSection(BaseRabbitMqConfig.SectionName));
      services.Configure<BaseServiceInfoConfig>(Configuration.GetSection(BaseServiceInfoConfig.SectionName));

      services.AddHttpContextAccessor();
      services
        .AddControllers()
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .AddNewtonsoftJson();


      string connStr = Environment.GetEnvironmentVariable("ConnectionString");
      if (string.IsNullOrEmpty(connStr))
      {
        connStr = Configuration.GetConnectionString("SQLConnectionString");

        Log.Information($"SQL connection string from appsettings.json was used. Value '{HidePassord(connStr)}'.");
      }
      else
      {
        Log.Information($"SQL connection string from environment was used. Value '{HidePassord(connStr)}'.");
      }

      services.AddDbContext<CompanyServiceDbContext>(options =>
      {
        options.UseSqlServer(connStr);
      });

      services.AddHealthChecks()
        .AddSqlServer(connStr)
        .AddRabbitMqCheck();

      string redisConnStr = Environment.GetEnvironmentVariable("RedisConnectionString");
      if (string.IsNullOrEmpty(redisConnStr))
      {
        redisConnStr = Configuration.GetConnectionString("Redis");

        Log.Information($"Redis connection string from appsettings.json was used. Value '{HidePassord(redisConnStr)}'");
      }
      else
      {
        Log.Information($"Redis connection string from environment was used. Value '{HidePassord(redisConnStr)}'");
      }

      services.AddSingleton<IConnectionMultiplexer>(
        x => ConnectionMultiplexer.Connect(redisConnStr));

      services.AddBusinessObjects();

      ConfigureMassTransit(services);
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
      UpdateDatabase(app);

      app.UseForwardedHeaders();

      app.UseExceptionsHandler(loggerFactory);

      app.UseApiInformation();

      app.UseRouting();

      app.UseMiddleware<TokenMiddleware>();

      app.UseCors(CorsPolicyName);

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers().RequireCors(CorsPolicyName);

        endpoints.MapHealthChecks($"/{_serviceInfoConfig.Id}/hc", new HealthCheckOptions
        {
          ResultStatusCodes = new Dictionary<HealthStatus, int>
          {
            { HealthStatus.Unhealthy, 200 },
            { HealthStatus.Healthy, 200 },
            { HealthStatus.Degraded, 200 },
          },
          Predicate = check => check.Name != "masstransit-bus",
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
      });
    }

    #endregion

    #region private methods

    private void ConfigureMassTransit(IServiceCollection services)
    {
      services.AddMassTransit(x =>
      {
        x.AddConsumer<GetPositionsConsumer>();
        x.AddConsumer<GetDepartmentsConsumer>();
        x.AddConsumer<EditCompanyEmployeeConsumer>();
        x.AddConsumer<GetDepartmentUsersConsumer>();
        x.AddConsumer<SearchDepartmentsConsumer>();
        x.AddConsumer<GetSmtpCredentialsConsumer>();
        x.AddConsumer<GetCompanyEmployeesConsumer>();
        x.AddConsumer<DisactivateUserConsumer>();
        x.AddConsumer<CheckDepartmentsExistenceConsumer>();

        x.UsingRabbitMq((context, cfg) =>
          {
            cfg.Host(_rabbitMqConfig.Host, "/", host =>
              {
                host.Username($"{_serviceInfoConfig.Name}_{_serviceInfoConfig.Id}");
                host.Password(_serviceInfoConfig.Id);
              });

            ConfigureEndpoints(context, cfg);
          });

        x.AddRequestClients(_rabbitMqConfig);
      });

      services.AddMassTransitHostedService();
    }

    private void ConfigureEndpoints(
      IBusRegistrationContext context,
      IRabbitMqBusFactoryConfigurator cfg)
    {
      cfg.ReceiveEndpoint(_rabbitMqConfig.GetPositionsEndpoint, ep =>
      {
        ep.ConfigureConsumer<GetPositionsConsumer>(context);
      });

      cfg.ReceiveEndpoint(_rabbitMqConfig.GetDepartmentsEndpoint, ep =>
      {
        ep.ConfigureConsumer<GetDepartmentsConsumer>(context);
      });

      cfg.ReceiveEndpoint(_rabbitMqConfig.GetDepartmentUsersEndpoint, ep =>
      {
        ep.ConfigureConsumer<GetDepartmentUsersConsumer>(context);
      });

      cfg.ReceiveEndpoint(_rabbitMqConfig.SearchDepartmentEndpoint, ep =>
      {
        ep.ConfigureConsumer<SearchDepartmentsConsumer>(context);
      });

      cfg.ReceiveEndpoint(_rabbitMqConfig.GetSmtpCredentialsEndpoint, ep =>
      {
        ep.ConfigureConsumer<GetSmtpCredentialsConsumer>(context);
      });

      cfg.ReceiveEndpoint(_rabbitMqConfig.GetCompanyEmployeesEndpoint, ep =>
      {
        ep.ConfigureConsumer<GetCompanyEmployeesConsumer>(context);
      });

      cfg.ReceiveEndpoint(_rabbitMqConfig.EditCompanyEmployeeEndpoint, ep =>
      {
        ep.ConfigureConsumer<EditCompanyEmployeeConsumer>(context);
      });

      cfg.ReceiveEndpoint(_rabbitMqConfig.DisactivateUserEndpoint, ep =>
      {
        ep.ConfigureConsumer<DisactivateUserConsumer>(context);
      });

      cfg.ReceiveEndpoint(_rabbitMqConfig.CheckDepartmentsExistenceEndpoint, ep =>
      {
        ep.ConfigureConsumer<CheckDepartmentsExistenceConsumer>(context);
      });
    }

    private void UpdateDatabase(IApplicationBuilder app)
    {
      using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();

      using var context = serviceScope.ServiceProvider.GetService<CompanyServiceDbContext>();

      context.Database.Migrate();
    }

    private string HidePassord(string line)
    {
      string password = "Password";

      int index = line.IndexOf(password, 0, StringComparison.OrdinalIgnoreCase);

      if (index != -1)
      {
        string[] words = Regex.Split(line, @"[=,; ]");

        for (int i = 0; i < words.Length; i++)
        {
          if (string.Equals(password, words[i], StringComparison.OrdinalIgnoreCase))
          {
            line = line.Replace(words[i + 1], "****");
            break;
          }
        }
      }

      return line;
    }

    #endregion
  }
}
