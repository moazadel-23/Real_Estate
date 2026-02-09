using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Models.ViewModel;
using Real_Estate.Repository;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate.Areas.Identity.Controllers.Account
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IEmailSender emailSender;
        private readonly IRepository<UserOtp> userOtpRepository;

        public AccountController(UserManager<User> _userManager, IEmailSender _emailSender, IRepository<UserOtp> _userOtpRepository)
        {
            userManager = _userManager;
            emailSender = _emailSender;
            userOtpRepository = _userOtpRepository;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM login)
        {

            var user = await userManager.FindByEmailAsync(login.Email);
            if (user is null)
            {
                TempData["LogInError"] = "خطأ في البريد الالكتروني او كلمة المرور";
                ViewBag.Email = login.Email;
                ViewBag.Password = login.Password;
                return View();
            }
            var result = userManager.CheckPasswordAsync(user, login.Password);
            if (!result.Result)
            {
                TempData["LogInError"] = "خطأ في البريد الالكتروني او كلمة المرور";
                ViewBag.Email = login.Email;
                ViewBag.Password = login.Password;
                return View();
            }

            return RedirectToAction("index", "Property", new { area = "Admin" });


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
            {
                TempData["EmailExists"] = "البريد الالكتروني مسجل بالفعل";
                ViewBag.Name = register.Name;
                ViewBag.Email = register.Email;
                return View();
            }
            var user = new User
            {
                FullName = register.Name,
                Email = register.Email,
                UserName = register.Email
            };
            var result = await userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    builder.AppendLine(error.Description);
                }
                TempData["ErrorInPassword"] = builder.ToString();
                ViewBag.Name = register.Name;
                ViewBag.Email = register.Email;
                return View();
            }
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(VirfeyEmail), "Account", new { userId = user.Id, userToken = token }, Request.Scheme);
            await emailSender.SendEmailAsync(register.Email, "Confirm Your Email", $"<h1>To Confirm Email Click<a href='{link}'> Here</a></h1>");
            return View();
        }
        public async Task<IActionResult> VirfeyEmail(string userId, string userToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            var result = await userManager.ConfirmEmailAsync(user!, userToken);
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
            var userOtp = new UserOtp
            {
                Otp = new Random().Next(1000, 9999),
                UserId = user.Id,
                CreatedAt = DateTime.Now,
                ExpireAt = DateTime.Now.AddMinutes(10)
            };
            await emailSender.SendEmailAsync(email, "Reset Password", $"<h1>Your OTP is {userOtp.Otp}</h1>");
            await userOtpRepository.AddAsync(userOtp);
            await userOtpRepository.CommitChange();
            return RedirectToAction(nameof(VerifyOtp), new { userId = user.Id, otp = userOtp.Otp });
        }
        [HttpGet]
        public async Task<IActionResult> VerifyOtp(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return View();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string userId, string otp)
        {
            var userOtp = await userOtpRepository.GetOneAsync(u => u.UserId == userId && u.Otp == int.Parse(otp));
            if (userOtp is null)
                return View();
            return RedirectToAction(nameof(ResetPassword), new { userId });
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId)
        {
            return View(new ResetPasswordVM { UserId = userId});
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            var user = await userManager.FindByIdAsync(resetPassword.UserId);
            var token = await userManager.GeneratePasswordResetTokenAsync(user!);
            var result = await userManager.ResetPasswordAsync(user!, token, resetPassword.Password);
            if (!result.Succeeded)
                return View();
            return RedirectToAction(nameof(LogIn));
        }
    }
}
