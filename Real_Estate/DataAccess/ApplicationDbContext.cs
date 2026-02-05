using Microsoft.EntityFrameworkCore;

namespace Real_Estate.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        DbSet<Property> Properties { get; set; }
        DbSet<PropertySubImage> PropertySubImage { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Favorite> favorites { get; set; }
        DbSet<User> users { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
