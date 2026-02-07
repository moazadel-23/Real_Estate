using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Models.ViewModel;
using System.Threading.Tasks;

namespace Real_Estate.Areas.Identity.Controllers.Account
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;

        public AccountController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM login)
        {

            if(!ModelState.IsValid)
                return View(login);

            var user = await userManager.FindByEmailAsync(login.Email);
            if (user is null)
            {
                ModelState.AddModelError("Email", "Invalid Email");
                return View(login);
            }

            //user.PasswordHash == login.Password;
            //var result = await SignInManager.PasswordSignInAsync(user, login.Password, false, false);
            var result=userManager.CheckPasswordAsync(user, login.Password);
            if (!result.Result)
            {
                ModelState.AddModelError("Password", "Invalid Password");
                return View(login);
            }

            return RedirectToAction("index", "home", new { area = "Property" });


        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM register)
        {
                if(!ModelState.IsValid)
                    return View(register);

            return View();
        }
    }
}
