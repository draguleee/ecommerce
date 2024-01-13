using ecommerce.Data.Services.Interfaces;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Data.Services.Classes
{
    public class OrdersService : IOrdersService
    {
        // Inject the AppDbContext 
        private readonly AppDbContext _context;
        public OrdersService(AppDbContext context)
        {
            _context = context;
        }

        // Get orders by user ID
        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string uerRole)
        {
            var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Movie).Include(n => n.User).ToListAsync();
            if (uerRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }
            return orders;
        }

        // Store the order
        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            foreach (var item in items) 
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    MovieId = item.Movie.Id,
                    OrderId = order.Id,
                    Price = item.Movie.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}
