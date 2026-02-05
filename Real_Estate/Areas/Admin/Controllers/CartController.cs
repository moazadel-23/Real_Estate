using Microsoft.AspNetCore.Mvc;

namespace Real_Estate.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
