using Microsoft.EntityFrameworkCore;

namespace streak.Models
{
    public class WorklogContext: DbContext
    {
        public WorklogContext(DbContextOptions<WorklogContext> options): base(options)
        {
            
        }

        public DbSet<WorklogItem> WorklogItems { get; set; } = null;
    }
}