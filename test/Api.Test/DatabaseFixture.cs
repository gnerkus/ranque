using Entities.Test;
using Microsoft.EntityFrameworkCore;
using Repository;
using Testcontainers.MsSql;

namespace Entities.Test
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();
        private RepositoryContext _dbContext = default!;
        public string MsSqlConnectionString => _dbContainer.GetConnectionString();

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            var dbContextOptions = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(MsSqlConnectionString, b => { b.MigrationsAssembly("Core"); })
                .Options;
            _dbContext = new RepositoryContext(dbContextOptions);
            await _dbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            if (_dbContext is not null) await _dbContext.DisposeAsync();

            if (_dbContainer is not null)
            {
                await _dbContainer.StopAsync();
                await _dbContainer.DisposeAsync();
            }
        }
    }
}

[CollectionDefinition("IntegrationTests")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply to be the place
    // to apply [CollectionDefinition] and all the ICollectionFixture<> interfaces.
}