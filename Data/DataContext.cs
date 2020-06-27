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

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=LubyDB;trusted_connection=true;");
        }*/

        // Objetos
        public DbSet<Task> Tasks { get; set; } 
        public DbSet<User> Users { get; set; }
    }
}