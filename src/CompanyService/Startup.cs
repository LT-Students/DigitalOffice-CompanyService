using FluentValidation;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.CompanyService.Broker.Consumers;
using LT.DigitalOffice.CompanyService.Business;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Mappers;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation;
using LT.DigitalOffice.Kernel;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Middlewares.Token;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LT.DigitalOffice.CompanyService
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddDbContext<CompanyServiceDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLConnectionString"));
            });

            services.AddControllers();

            services.AddKernelExtensions();

            ConfigureCommands(services);
            ConfigureRepositories(services);
            ConfigureValidators(services);
            ConfigureMappers(services);
            ConfigureMassTransit(services);
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            var rabbitMqConfig = Configuration.GetSection(BaseRabbitMqOptions.RabbitMqSectionName).Get<BaseRabbitMqOptions>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetUserPositionConsumer>();
                x.AddConsumer<GetDepartmentConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConfig.Host, "/", hst =>
                    {
                        hst.Username($"{rabbitMqConfig.Username}_{rabbitMqConfig.Password}");
                        hst.Password(rabbitMqConfig.Password);
                    });

                    cfg.ReceiveEndpoint($"{rabbitMqConfig.Username}", e =>
                    {
                        e.ConfigureConsumer<GetUserPositionConsumer>(context);
                    });

                    cfg.ReceiveEndpoint($"{rabbitMqConfig.Username}_Departments", e =>
                    {
                        e.ConfigureConsumer<GetDepartmentConsumer>(context);
                    });
                });

                x.AddRequestClient<ICheckTokenRequest>(new Uri(rabbitMqConfig.ValidateTokenEndpoint));

                x.ConfigureKernelMassTransit(rabbitMqConfig);
            });

            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/healthcheck");

            app.UseExceptionHandler(tempApp => tempApp.Run(CustomExceptionHandler.HandleCustomException));

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

        private void ConfigureCommands(IServiceCollection services)
        {
            services.AddTransient<IGetPositionByIdCommand, GetPositionByIdCommand>();
            services.AddTransient<IGetPositionsListCommand, GetPositionsListCommand>();
            services.AddTransient<IEditPositionCommand, EditPositionCommand>();
            services.AddTransient<IDisablePositionByIdCommand, DisablePositionByIdCommand>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, CompanyServiceDbContext>();

            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IPositionRepository, PositionRepository>();
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<Position>, PositionValidator>();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IMapper<DbPosition, PositionResponse>, PositionMapper>();
            services.AddTransient<IMapper<Position, DbPosition>, PositionMapper>();
        }
    }
}