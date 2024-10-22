using Microsoft.EntityFrameworkCore;
using Repository;
using Testcontainers.MsSql;

namespace Entities.Test
{
    public class SharedFixture: IAsyncLifetime
    {
        public RepositoryContext DbContext = default!;
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

        private const string Database = "ranque";
        private const string Username = "sa";
        private const string Password = "$trongPassword";
        private const ushort MsSqlPort = 1433;

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            var host = _dbContainer.Hostname;
            var port = _dbContainer.GetMappedPublicPort(MsSqlPort);

            var connectionString =
                $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True";
            var dbContextOptions = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(connectionString)
                .Options;
            DbContext = new RepositoryContext(dbContextOptions);
            await DbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            if (DbContext is not null)
            {
                await DbContext.DisposeAsync();
            }

            if (_dbContainer is not null)
            {
                await _dbContainer.StopAsync();
                await _dbContainer.DisposeAsync();
            }
        }
    }
}