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
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

            Version = "1.4.0";
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
            }

            services.AddDbContext<CompanyServiceDbContext>(options =>
            {
                options.UseSqlServer(connStr);
            });

            services.AddHealthChecks()
                .AddSqlServer(connStr)
                .AddRabbitMqCheck();

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
                x.AddConsumer<GetPositionConsumer>();
                x.AddConsumer<GetDepartmentConsumer>();
                x.AddConsumer<GetUserDepartmentConsumer>();
                x.AddConsumer<FindDepartmentsConsumer>();
                x.AddConsumer<ChangeUserDepartmentConsumer>();
                x.AddConsumer<ChangeUserPositionConsumer>();
                x.AddConsumer<FindDepartmentUsersConsumer>();
                x.AddConsumer<SearchDepartmentsConsumer>();
                x.AddConsumer<ChangeUserOfficeConsumer>();
                x.AddConsumer<GetSmtpCredentialsConsumer>();

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
            cfg.ReceiveEndpoint(_rabbitMqConfig.GetPositionEndpoint, ep =>
            {
                ep.ConfigureConsumer<GetPositionConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.GetDepartmentEndpoint, ep =>
            {
                ep.ConfigureConsumer<GetDepartmentConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.GetDepartmentUserEndpoint, ep =>
            {
                ep.ConfigureConsumer<GetUserDepartmentConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.FindDepartmentsEndpoint, ep =>
            {
                ep.ConfigureConsumer<FindDepartmentsConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.FindDepartmentUsersEndpoint, ep =>
            {
                ep.ConfigureConsumer<FindDepartmentUsersConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.ChangeUserDepartmentEndpoint, ep =>
            {
                ep.ConfigureConsumer<ChangeUserDepartmentConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.ChangeUserPositionEndpoint, ep =>
            {
                ep.ConfigureConsumer<ChangeUserPositionConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.SearchDepartmentEndpoint, ep =>
            {
                ep.ConfigureConsumer<SearchDepartmentsConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.ChangeUserOfficeEndpoint, ep =>
            {
                ep.ConfigureConsumer<ChangeUserOfficeConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.GetSmtpCredentialsEndpoint, ep =>
            {
                ep.ConfigureConsumer<GetSmtpCredentialsConsumer>(context);
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

        #endregion
    }
}