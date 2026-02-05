using Microsoft.EntityFrameworkCore;
using Real_Estate.Repository;

namespace Real_Estate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

        
            builder.Services.AddControllersWithViews();

            // Configure DbContext with SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

          
            builder.Services.AddScoped<IRepository<Property>, Repository<Property>>();
            builder.Services.AddScoped<IRepository<Location>, Repository<Location>>();
         

            var app = builder.Build();

         
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

           
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Property}/{controller=Home}/{action=Index}/{id?}");

          
            app.Run();
        }
    }
}
