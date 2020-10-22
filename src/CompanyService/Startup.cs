using CompanyService.Mappers;
using FluentValidation;
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
using LT.DigitalOffice.CompanyService.Validation;
using LT.DigitalOffice.Kernel;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.Configure<RabbitMQOptions>(Configuration);

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
            var rabbitMQOptions = Configuration.GetSection(RabbitMQOptions.RabbitMQ).Get<RabbitMQOptions>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetUserPositionConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQOptions.Host, "/", hst =>
                    {
                        hst.Username($"{rabbitMQOptions.Username}_{rabbitMQOptions.Password}");
                        hst.Password(rabbitMQOptions.Password);
                    });

                    cfg.ReceiveEndpoint($"{rabbitMQOptions.Username}", e =>
                    {
                        e.ConfigureConsumer<GetUserPositionConsumer>(context);
                    });
                });

                x.ConfigureKernelMassTransit(rabbitMQOptions);
            });
        }

            public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/healthcheck");

            app.UseExceptionHandler(tempApp => tempApp.Run(CustomExceptionHandler.HandleCustomException));

            UpdateDatabase(app);

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
            services.AddTransient<IGetCompanyByIdCommand, GetCompanyByIdCommand>();
            services.AddTransient<IGetCompaniesListCommand, GetCompaniesListCommand>();
            services.AddTransient<IAddCompanyCommand, AddCompanyCommand>();

            services.AddTransient<IGetPositionByIdCommand, GetPositionByIdCommand>();
            services.AddTransient<IGetPositionsListCommand, GetPositionsListCommand>();
            services.AddTransient<IAddPositionCommand, AddPositionCommand>();
            services.AddTransient<IEditPositionCommand, EditPositionCommand>();
            services.AddTransient<IDisablePositionByIdCommand, DisablePositionByIdCommand>();

            services.AddTransient<IAddDepartmentCommand, AddDepartmentCommand>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, CompanyServiceDbContext>();

            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IPositionRepository, PositionRepository>();
        }

        private void ConfigureValidators(IServiceCollection services)
        {
            services.AddTransient<IValidator<AddCompanyRequest>, AddCompanyValidator>();
            services.AddTransient<IValidator<EditCompanyRequest>, EditCompanyValidator>();

            services.AddTransient<IValidator<AddPositionRequest>, AddPositionRequestValidator>();
            services.AddTransient<IValidator<EditPositionRequest>, EditPositionRequestValidator>();

            services.AddTransient<IValidator<DepartmentRequest>, DepartmentRequestValidator>();
        }

        private void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IMapper<DbCompany, Company>, CompanyMapper>();
            services.AddTransient<IMapper<AddCompanyRequest, DbCompany>, CompanyMapper>();
            services.AddTransient<IMapper<EditCompanyRequest, DbCompany>, CompanyMapper>();

            services.AddTransient<IMapper<DbPosition, Position>, PositionMapper>();
            services.AddTransient<IMapper<AddPositionRequest, DbPosition>, PositionMapper>();
            services.AddTransient<IMapper<EditPositionRequest, DbPosition>, PositionMapper>();

            services.AddTransient<IMapper<DepartmentRequest, DbDepartment>, DepartmentMapper>();
        }
    }
}