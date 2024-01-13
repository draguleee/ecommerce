using ecommerce.Data.Cart;
using ecommerce.Data.Roles;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ecommerce.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        // Injections
        private readonly IMoviesService _moviesService;         // Movies service
        private readonly ShoppingCart _shoppingCart;            // Shopping cart
        private readonly IOrdersService _ordersService;         // Orders service

        // Constructor for OrdersController controller
        public OrdersController(IMoviesService moviesService, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            _moviesService = moviesService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
        }

        // GET: orders
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }

        // GET: orders/shoppingcart
        [HttpGet]
        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(response);
        }

        // POST: orders/additemstoshoppingcart
        [HttpPost]
        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);
            if (item != null) _shoppingCart.AddItemToCart(item);
            return RedirectToAction(nameof(ShoppingCart));
        }

        // POST: orders/removeitemfromshoppingcart
        [HttpPost]
        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _moviesService.GetMovieByIdAsync(id);
            if (item != null) _shoppingCart.RemoveItemFromCart(item);
            return RedirectToAction(nameof(ShoppingCart));
        }

        // POST: orders/completeorder
        [HttpPost]
        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);
            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();
            return View("OrderCompleted");
        }
    }
}
