using Microsoft.AspNetCore.Identity;

namespace Real_Estate.DI_Service
{
    public class AddService
    {
        public void AddScopedServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        }
    }
}
