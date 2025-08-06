using Microsoft.EntityFrameworkCore;
using StreamFlixAPI.Models;

namespace StreamFlixAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }

    }
}
