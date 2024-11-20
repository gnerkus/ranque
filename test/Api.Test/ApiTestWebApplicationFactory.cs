using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Entities.Test
{
    public class ApiTestWebApplicationFactory(DatabaseFixture fixture)
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // Find the existing db context and remove it
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof
                    (DbContextOptions<RepositoryContext>));

                if (dbContextDescriptor is not null) services.Remove(dbContextDescriptor);

                var ctx = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(RepositoryContext));
                services.Remove(ctx!);

                services.AddDbContext<RepositoryContext>((container, options) =>
                    {
                        options.UseSqlServer(fixture.MsSqlConnectionString);
                    })
                    .AddSingleton<IStartupFilter>(new AutoAuthorizeStartupFilter());
            });
        }
    }
}