using Microsoft.AspNetCore.Mvc;
using Real_Estate.Models.ViewModel;

namespace Real_Estate.Areas.Identity.Controllers.Account
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(LoginVM login)
        {

            return View();
        }
    }
}
