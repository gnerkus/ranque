using Entities;
using Entities.Models;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Leaderboard>()
                .HasMany(e => e.Participants)
                .WithMany(e => e.Leaderboards)
                .UsingEntity<Score>();

            builder.ApplyConfiguration(new OrganizationConfiguration());
            builder.ApplyConfiguration(new LeaderboardConfiguration());
            builder.ApplyConfiguration(new ParticipantConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}