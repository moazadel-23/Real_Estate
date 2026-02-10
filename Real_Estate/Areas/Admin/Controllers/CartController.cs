using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.Repository;
using System.Threading.Tasks;

namespace Real_Estate.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Cart> _cartRepository;
     

        public CartController(UserManager<User> userManager, IRepository<Cart> cartRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            
        }

        public async Task<IActionResult> AddToCart(int propertyId, int count,CancellationToken cancellationToken)
        {
            var userId = _userManager.GetUserId(User);
            if(userId == null)
                return NotFound();
            await _cartRepository.AddAsync(new Cart
            {
                PropertyId = propertyId,
                UserId = userId,
                Count = count,
                Price = 0 
            }, cancellationToken);
            await _cartRepository.CommitAsync(cancellationToken);
            TempData["Success"] = "Property added to cart successfully.";
            return RedirectToAction("Index", "Property");

        }
    }
}