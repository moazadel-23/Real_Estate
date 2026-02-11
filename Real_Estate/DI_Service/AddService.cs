using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Real_Estate.Email_Service;
using Real_Estate.Repository;

namespace Real_Estate.DI_Service
{
    public static class AddService
    {
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IRepository<Property>, Repository<Property>>();
            services.AddScoped<IRepository<Location>, Repository<Location>>();
            services.AddScoped<IRepository<PropertySubImage>, Repository<PropertySubImage>>();
            services.AddScoped<IRepository<UserOtp>, Repository<UserOtp>>();
            services.AddScoped<IRepository<Favorite>, Repository<Favorite>>();
            services.AddTransient<IEmailSender, EmailSender>();

        }
    }
}
