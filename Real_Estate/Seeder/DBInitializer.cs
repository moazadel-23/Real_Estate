using Microsoft.AspNetCore.Identity;

namespace Real_Estate.Seeder
{
    public class DBInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DBInitializer> _logger;

        public DBInitializer(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationDbContext dbContext, ILogger<DBInitializer> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }
        public void Initialize()
        {
            try
            {
                if(_dbContext.Database.GetPendingMigrations().Any())
                    _dbContext.Database.Migrate();
                if(!_roleManager.Roles.Any())
                {
                    _roleManager.CreateAsync(new(Role.SuperAdmin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(Role.Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(Role.User)).GetAwaiter().GetResult();
                }
                _userManager.CreateAsync(new()
                {
                    FullName = "Super Admin",
                    UserName = "superadmin",
                    Email = "SuperAdmin4@gmail.com",
                }, "Superadmin12369").GetAwaiter().GetResult();
                var user = _userManager.FindByEmailAsync("SuperAdmin4@gmail.com").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user! , Role.SuperAdmin).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
