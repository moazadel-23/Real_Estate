using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Models;
using Real_Estate.Repository;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Real_Estate.Controllers 
{
    public class FavoriteController : Controller
    {
        private readonly IRepository<Favorite> _repository;

        public FavoriteController(IRepository<Favorite> repository)
        {
            _repository = repository;
        }

        private string? GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var favorites = await _repository.GetAllAsync(
                f => f.UserId == userId,
                include: new Expression<Func<Favorite, object>>[] { f => f.Property },
                tracking: false
            );

            var properties = favorites?
                .Where(f => f.Property != null)
                .Select(f => f.Property)
                .ToList() ?? new List<Property>();

            return View(properties);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int propertyId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            try
            {
             
                var exists = await _repository.GetOneAsync(
                    f => f.UserId == userId && f.PropertyId == propertyId
                );

                if (exists != null && exists.Id != 0)
                {
                    TempData["Message"] = "Property is already in favorites.";
                    return RedirectToAction("Details", "Property", new { id = propertyId });
                }

                var favorite = new Favorite
                {
                    UserId = userId, 
                    PropertyId = propertyId
                };

                await _repository.AddAsync(favorite);
                await _repository.CommitAsync(default);

                TempData["Success"] = "Added to favorites successfully.";
                return RedirectToAction("Details", "Property", new { id = propertyId });
            }
            catch
            {
                TempData["Error"] = "Error adding favorite.";
                return RedirectToAction("Details", "Property", new { id = propertyId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var favorite = await _repository.GetOneAsync(f => f.Id == id);

                if (favorite == null) return NotFound();

                _repository.Delete(favorite);
                await _repository.CommitAsync(default);

                TempData["Success"] = "Removed from favorites.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int propertyId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId)) return Json(new { success = false, message = "Login required" });

            try
            {
                var fav = await _repository.GetOneAsync(f => f.UserId == userId && f.PropertyId == propertyId);

                if (fav != null && fav.Id != 0)
                {
                    _repository.Delete(fav);
                    await _repository.CommitAsync(default);
                    return Json(new { success = true, action = "removed" });
                }
                else
                {
                    var newFav = new Favorite { UserId = userId, PropertyId = propertyId };
                    await _repository.AddAsync(newFav);
                    await _repository.CommitAsync(default);
                    return Json(new { success = true, action = "added" });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Error occurred" });
            }
        }
    }
}