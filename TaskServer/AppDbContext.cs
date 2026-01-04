using Microsoft.EntityFrameworkCore;

namespace TaskServer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
    }
}
