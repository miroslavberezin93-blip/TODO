using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.DATA
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<User> Users => Set<User>();
    }
}
