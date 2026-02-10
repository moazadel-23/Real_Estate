using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Models;
using Real_Estate.Models.ViewModel;
using System.Diagnostics;

namespace Real_Estate.Areas.Property.Controllers
{
    [Area("Property")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(PropertyFilterVM filterModel)
        {
            var query = _context.Set<Real_Estate.Models.Property>() // <-- FIXED HERE
                                .Include(p => p.Location)
                                .AsQueryable();

            if (filterModel.PropertyType.HasValue)
            {
                query = query.Where(p => p.Type == filterModel.PropertyType.Value);
            }

            if (filterModel.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filterModel.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(filterModel.SearchLocation))
            {
                query = query.Where(p => p.Location != null &&
                                         p.Location.City.Contains(filterModel.SearchLocation));
            }

            filterModel.Properties = await query.ToListAsync();

            return View(filterModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
