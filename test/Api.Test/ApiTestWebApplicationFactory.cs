using System.Data.Common;
using Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Entities.Test;
    
public class ApiTestWebApplicationFactory<TProgram>: WebApplicationFactory<TProgram> where 
TProgram: class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Find the existing dbcontext and remove it
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof
                (DbContextOptions<RepositoryContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof
                (DbConnection));

            services.Remove(dbConnectionDescriptor);
            
            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            // Use SQLite as in-memory database for the tests
            services.AddDbContext<RepositoryContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });

        builder.UseEnvironment("Development");
    }
}
