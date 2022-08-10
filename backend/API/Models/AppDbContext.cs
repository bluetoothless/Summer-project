using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<BarberingService> BarberingServices { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
