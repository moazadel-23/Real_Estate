using Microsoft.EntityFrameworkCore;

namespace Real_Estate.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        DbSet<Property> Properties { get; set; }
        DbSet<PropertyImage> PropertyImages { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Favorite> Favorites { get; set; }
        DbSet<User> Users { get; set; }



        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog= Real_Estate_G10;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");

        }
    }
}
