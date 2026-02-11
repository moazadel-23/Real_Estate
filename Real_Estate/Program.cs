using Real_Estate.DI_Service;
using Real_Estate.Seeder;

namespace Real_Estate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

        
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Add services to the container.
            builder.Services.AddScopedServices();

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetService<DBInitializer>();
            dbInitializer?.Initialize();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

           
            app.MapControllerRoute(
         name: "areas",
         pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Property}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
