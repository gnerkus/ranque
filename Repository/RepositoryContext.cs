using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Organization>? Organizations { get; set; }
        public DbSet<Participant>? Participants { get; set; }
        public DbSet<Score>? Scores { get; set; }
        public DbSet<Leaderboard>? Leaderboards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Leaderboard>()
                .HasMany(e => e.Participants)
                .WithMany(e => e.Leaderboards)
                .UsingEntity<Score>();

            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new LeaderboardConfiguration());
            modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}