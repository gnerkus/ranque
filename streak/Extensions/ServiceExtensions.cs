using Asp.Versioning;
using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;

namespace streak.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination")
                );
            });
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration
            config)
        {
            services.AddDbContext<RepositoryContext>(opts =>
                opts.UseSqlServer(config.GetConnectionString("sqlConnection")));
        }


        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => { });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(config =>
                config.OutputFormatters.Add(new CsvOutputFormatter()));
        }

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                    .OfType<SystemTextJsonOutputFormatter>()?
                    .FirstOrDefault();
                if (systemTextJsonOutputFormatter != null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.nanotome.hateoas+json");
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.nanotome.apiroot+json");
                }
                var xmlOutputFormatter = config.OutputFormatters
                    .OfType<XmlDataContractSerializerOutputFormatter>()?
                    .FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.nanotome.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.nanotome.apiroot+xml");
                }
            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            }).AddMvc();
        }
    }
}