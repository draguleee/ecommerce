using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace ecommerce.Data.Cart
{
    public class ShoppingCart
    {
        public AppDbContext _context { get; set; }
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        // Constructor pentru ShoppingCart
        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }

        // Metoda care obtine / creeaza un cos de cumparaturi
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        // Metoda care adauga un film in cosul de cumparaturi
        // Daca filmul deja exista in cos, se creste cantitatea
        public void AddItemToCart(Movie movie)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCartItem == null) 
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };
            _context.ShoppingCartItems.Add(shoppingCartItem);
            } 
            else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }

        // Metoda care elimina un film din cosul de cumparaturi
        // Daca cantitatea este mai mare ca 1, se decrementeaza 
        public void RemoveItemFromCart(Movie movie) 
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1) shoppingCartItem.Amount--;
                else _context.ShoppingCartItems.Remove(shoppingCartItem);
            }
            _context.SaveChanges();
        }

        // Metoda care returneaza elementele din cosul de cumparaturi
        // Se incarca elementele in baza de date daca nu sunt deja incarcate
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Movie).ToList());
        }

        // Metoda care returneaza totalul cosului de cumparaturi
        // Se inmulteste pretul fiecarui film cu cantitatea adaugata in cos
        public double GetShoppingCartTotal() 
        {
            return _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Movie.Price * n.Amount).Sum();
        }

        // Metoda care goleste continutul cosului de cumparaturi
        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();  
        }
    }
}
