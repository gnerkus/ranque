using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace streak.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            var connectionString = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");

            if (string.IsNullOrWhiteSpace(connectionString) && environment == "Production")
            {
                connectionString =
                    Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
            }
            
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly("streak")
                );

            return new RepositoryContext(builder.Options);
        }
    }
}