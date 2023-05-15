using Microsoft.EntityFrameworkCore;
using WebAPITest.Models;

namespace WebAPITest.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Planning> Plannings => Set<Planning>();
        public DbSet<Card> Cards => Set<Card>();
        public DbSet<Servicio> Servicios => Set<Servicio>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Planning>().HasKey(pl => new { pl.Dia, pl.Branch, pl.GroupName });
        }
    }
}