using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Testcontainers.MsSql;

namespace Entities.Test;
    
public class ApiTestWebApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    private RepositoryContext _dbContext = default!;
    private string MsSqlConnectionString => _dbContainer.GetConnectionString();
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Find the existing db context and remove it
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof
                (DbContextOptions<RepositoryContext>));

            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Use SQLite as in-memory database for the tests
            services.AddDbContext<RepositoryContext>((container, options) =>
            {
                options.UseSqlServer(MsSqlConnectionString);
            });
        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var dbContextOptions = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(MsSqlConnectionString, b =>
            {
                b.EnableRetryOnFailure();
                b.MigrationsAssembly("Core");
            })
            .Options;
        _dbContext = new RepositoryContext(dbContextOptions);
        await _dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        if (_dbContext is not null)
        {
            await _dbContext.DisposeAsync();
        }

        if (_dbContainer is not null)
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }
    }
}
