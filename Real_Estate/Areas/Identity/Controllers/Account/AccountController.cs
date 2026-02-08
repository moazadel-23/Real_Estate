using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Models.ViewModel;
using System.Threading.Tasks;

namespace Real_Estate.Areas.Identity.Controllers.Account
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<User> _userManager, IEmailSender _emailSender)
        {
            userManager = _userManager;
            emailSender = _emailSender;
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
        public async Task<IActionResult> Register(RegisterVM register)
        {
            var emailIfExite = await userManager.FindByEmailAsync(register.Email);
            if (emailIfExite is not null)
                return View();
            var user = new User
            {
                FullName = register.Name,
                Email = register.Email,
                UserName = register.Email
            };
            var result = await userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
                return View();
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(VirfeyEmail), "Account", new { userId = user.Id , userToken = token } , Request.Scheme);
            await emailSender.SendEmailAsync(register.Email, "Confirm Your Email", $"<h1>To Confirm Email Click<a href='{link}'> Here</a></h1>");
            return View();
        }
        public async Task<IActionResult> VirfeyEmail(string userId , string userToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            var result = await userManager.ConfirmEmailAsync(user! , userToken);
            if (!result.Succeeded)
                return View();
            return RedirectToAction(nameof(LogIn));
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return View();
            return View();
        }
    }
}
