using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Real_Estate.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class FavoriteController : Controller
    {
        [HttpGet]
        public IActionResult favPage()
        {
            return View();
        }
    }
}
