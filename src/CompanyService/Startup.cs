using HealthChecks.UI.Client;
using LT.DigitalOffice.CompanyService.Broker.Consumers;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.Extensions;
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

namespace LT.DigitalOffice.CompanyService
{
    public class Startup
    {
        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly BaseServiceInfoConfig _serviceInfoConfig;
        private readonly ILogger<Startup> _logger;

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

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("LT.DigitalOffice.CompanyService.Startup", LogLevel.Trace)
                    .AddConsole();
            });

            _logger = loggerFactory.CreateLogger<Startup>();
        }

        #region public methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenConfiguration>(Configuration.GetSection("CheckTokenMiddleware"));
            services.Configure<BaseRabbitMqConfig>(Configuration.GetSection(BaseRabbitMqConfig.SectionName));
            services.Configure<BaseServiceInfoConfig>(Configuration.GetSection(BaseServiceInfoConfig.SectionName));

            services.AddHttpContextAccessor();
            services.AddControllers();

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

            services.AddBusinessObjects(_logger);

            ConfigureMassTransit(services);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionsHandler(loggerFactory);

            UpdateDatabase(app);

            app.UseMiddleware<TokenMiddleware>();

#if RELEASE
            app.UseHttpsRedirection();
#endif

            app.UseRouting();

            string corsUrl = Configuration.GetSection("Settings")["CorsUrl"];

            app.UseCors(builder =>
                builder
                    .WithOrigins(corsUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

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
                x.AddConsumer<GetUserPositionConsumer>();
                x.AddConsumer<GetDepartmentConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_rabbitMqConfig.Host, "/", host =>
                    {
                        host.Username($"{_serviceInfoConfig.Name}_{_serviceInfoConfig.Id}");
                        host.Password(_serviceInfoConfig.Id);
                    });

                    ConfigureEndpoints(context, cfg);
                });

                x.AddRequestClients(_rabbitMqConfig, _logger);
            });

            services.AddMassTransitHostedService();
        }

        private void ConfigureEndpoints(
            IBusRegistrationContext context,
            IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint(_rabbitMqConfig.GetUserPositionEndpoint, ep =>
            {
                ep.ConfigureConsumer<GetUserPositionConsumer>(context);
            });

            cfg.ReceiveEndpoint(_rabbitMqConfig.GetDepartmentEndpoint, ep =>
            {
                ep.ConfigureConsumer<GetDepartmentConsumer>(context);
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