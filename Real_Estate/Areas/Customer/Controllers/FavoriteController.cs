using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Real_Estate.Models;
using Real_Estate.Repository;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Real_Estate.Models;

namespace Real_Estate.Controllers 
{
    [Area("Customer")]
    [Authorize]
    public class FavoriteController : Controller
    {
        public UserManager<User> _userManager { get; }
        public IRepository<Favorite> _favoriteRepository { get; }
        public IRepository<Models.Property> _propertyRepository { get; }

        public FavoriteController(UserManager<User> userManager, IRepository<Favorite> favoriteRepository, IRepository<Models.Property> propertyRepository )
        {
            _userManager = userManager;
            _favoriteRepository = favoriteRepository;
            _propertyRepository = propertyRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            var favorites = await _favoriteRepository.GetAllAsync(
                f => f.UserId == user.Id,
                include: new Expression<Func<Favorite, object>>[] { f => f.Property }
            );

            var properties = favorites
                .Where(f => f.Property != null)
                .Select(f => f.Property)
                .ToList();

            return View(properties);
        }

        public async Task<IActionResult> AddToFavorite(int PropertyId, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user is null)
                return NotFound();

            await _favoriteRepository.AddAsync(new Favorite
            {
                PropertyId = PropertyId,
                UserId = user.Id
            });
            await _favoriteRepository.CommitChange(cancellationToken : cancellationToken);
            TempData["Message"] = "Property added to favorites.";
            return RedirectToAction("Index");
        }
    }
}