using Microsoft.AspNetCore.Mvc;
using Real_Estate.Repository;


namespace Real_Estate.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PropertyController : Controller
    {
        private readonly IRepository<Models.Property> _propertyRepository;

        public PropertyController(IRepository<Models.Property> propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return View(properties.AsEnumerable());
        }
    }
}
