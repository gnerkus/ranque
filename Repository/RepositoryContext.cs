using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository;

public class RepositoryContext: DbContext
{
    public RepositoryContext(DbContextOptions options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
        modelBuilder.ApplyConfiguration(new LeaderboardConfiguration());
        modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
    }

    public DbSet<Organization>? Organizations { get; set; }
    public DbSet<Participant>? Participants { get; set; }
}