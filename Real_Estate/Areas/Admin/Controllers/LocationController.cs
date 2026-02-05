using Microsoft.AspNetCore.Mvc;
using Real_Estate.Models;
using Real_Estate.Repository;

namespace Real_Estate.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocationController : Controller
    {
        private readonly IRepository<Location> _locationRepository;

        public LocationController(IRepository<Location> locationRepository)
        {
            _locationRepository = locationRepository;
        }

        // GET: Admin/Location
        public async Task<IActionResult> Index()
        {
            var locations = await _locationRepository.GetAllAsync();
            return View(locations);
        }

        // POST: Admin/Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Location location)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            await _locationRepository.AddAsync(location);
            await _locationRepository.CommitChange();

            return Json(new { success = true, message = "تمت الإضافة بنجاح" });
        }

        // POST: Admin/Location/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] Location location)
        {
            if (location == null || location.Id == 0)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            if (!ModelState.IsValid)
                return Json(new { success = false, message = "بيانات غير صالحة" });

            _locationRepository.Update(location);
            await _locationRepository.CommitChange();

            return Json(new { success = true, message = "تم التعديل بنجاح" });
        }

        // POST: Admin/Location/Delete (إضفتها لكي تكون الصفحة كاملة)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _locationRepository.GetOneAsync(l => l.Id == id);
            if (location == null)
                return Json(new { success = false, message = "الموقع غير موجود" });

            _locationRepository.Delete(location);
            await _locationRepository.CommitChange();

            return Json(new { success = true, message = "تم الحذف بنجاح" });
        }
    }
}