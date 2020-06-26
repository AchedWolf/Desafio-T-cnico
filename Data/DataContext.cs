using Microsoft.EntityFrameworkCore;
using Luby.Models;

namespace Luby.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        // Objetos
        public DbSet<Task> Tasks { get; set; } 
        public DbSet<User> Users { get; set; }
    }
}