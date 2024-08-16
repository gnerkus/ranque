﻿namespace streak.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.Configure<IISOptions>(options =>
            {
                
            });
        }
    }
}