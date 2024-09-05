using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryContext: DbContext
{
    public RepositoryContext(DbContextOptions options): base(options)
    {
        
    }

    public DbSet<Organization>? Organizations { get; set; }
    public DbSet<Participant>? Participants { get; set; }
}