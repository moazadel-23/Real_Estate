using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Models;
using Real_Estate.Models.ViewModel;
using System.Threading.Tasks;

namespace Real_Estate.Areas.Identity.Controllers.Account
{
    [Area("Identity")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> userManager;

        public ProfileController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("LogIn", "Account");

            var model = new ProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber ?? "",
                Address = user.Address
            };

            return View(model);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Index(ProfileVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("LogIn", "Account");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            await userManager.UpdateAsync(user);

            TempData["Success"] = "تم تحديث البيانات بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
