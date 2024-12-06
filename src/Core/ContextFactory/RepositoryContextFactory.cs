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
                .AddEnvironmentVariables()
                .Build();
            var connectionString = configuration["AZURE_SQL_CONNECTIONSTRING"];

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(
                    connectionString,
                    b =>
                    {
                        b.EnableRetryOnFailure();
                        b.MigrationsAssembly("Core");
                    });

            return new RepositoryContext(builder.Options);
        }
    }
}