using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace streak.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_AZURE_SQL_CONNECTIONSTRING");

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