using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookDomain.Model;
using BookInfrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookInfrastructure.Controllers
{
    public class ShoppingBasketsController : Controller
    {
        private readonly BooksShopdatabaseContext _context;

        public ShoppingBasketsController(BooksShopdatabaseContext context)
        {
            _context = context;
        }

        // GET: ShoppingBaskets
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var baskets = isAdmin
                ? await _context.ShoppingBaskets
                    .Include(b => b.ShoppingBasketBooks)
                    .ThenInclude(sb => sb.Book)
                    .Include(b => b.Customer)
                    .ToListAsync()
                : await _context.ShoppingBaskets
                    .Include(b => b.ShoppingBasketBooks)
                    .ThenInclude(sb => sb.Book)
                    .Where(b => b.CustomerId == userId)
                    .ToListAsync();

            return View(baskets);
        }

        public async Task<IActionResult> AddToBasket(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return NotFound();

            var basket = await _context.ShoppingBaskets
                .Include(b => b.ShoppingBasketBooks)
                .FirstOrDefaultAsync(b => b.CustomerId == userId);

            if (basket == null)
            {
                basket = new ShoppingBasket { CustomerId = userId, ShoppingBasketBooks = new List<ShoppingBasketBook>() };
                _context.ShoppingBaskets.Add(basket);
            }

            var basketItem = basket.ShoppingBasketBooks.FirstOrDefault(sb => sb.BookId == bookId);
            if (basketItem != null)
            {
                basketItem.Count++;
            }
            else
            {
                 basket.ShoppingBasketBooks.Add(new ShoppingBasketBook { BookId = bookId, Count = 1, ShoppingBasketId = basket.Id });


            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int basketId, int bookId, decimal unitPrice, string action)
        {
            var basket = await _context.ShoppingBaskets
                .Include(sb => sb.ShoppingBasketBooks)
                .ThenInclude(sbb => sbb.Book)
                .FirstOrDefaultAsync(sb => sb.Id == basketId);

            if (basket == null)
            {
                return NotFound("Кошик не знайдено");
            }

            var item = basket.ShoppingBasketBooks.FirstOrDefault(sbb => sbb.BookId == bookId);
            if (item != null)
            {
                if (action == "increase")
                {
                    item.Count++;
                }
                else if (action == "decrease" && item.Count > 1)
                {
                    item.Count--;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Видалення книги
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBook(int basketId, int bookId)
        {
            var basket = await _context.ShoppingBaskets
                .Include(sb => sb.ShoppingBasketBooks)
                .ThenInclude(sbb => sbb.Book)
                .FirstOrDefaultAsync(sb => sb.Id == basketId);

            if (basket == null)
            {
                return NotFound("Кошик не знайдено");
            }

            var item = basket.ShoppingBasketBooks.FirstOrDefault(sbb => sbb.BookId == bookId);
            if (item != null)
            {
                _context.ShoppingBasketBooks.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ShoppingBaskets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBasket = await _context.ShoppingBaskets
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBasket == null)
            {
                return NotFound();
            }

            return View(shoppingBasket);
        }

        // GET: ShoppingBaskets/Create
        public IActionResult Create()
        {
            ViewData["CustomerEmail"] = new SelectList(_context.Set<Customer>(), "Id", "Id");
            return View();
        }

        // POST: ShoppingBaskets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerEmail")] ShoppingBasket shoppingBasket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingBasket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerEmail"] = new SelectList(_context.Set<Customer>(), "Id", "Id", shoppingBasket.CustomerId);
            return View(shoppingBasket);
        }

        // GET: ShoppingBaskets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBasket = await _context.ShoppingBaskets.FindAsync(id);
            if (shoppingBasket == null)
            {
                return NotFound();
            }
            ViewData["CustomerEmail"] = new SelectList(_context.Set<Customer>(), "Id", "Id", shoppingBasket.CustomerId);
            return View(shoppingBasket);
        }

        // POST: ShoppingBaskets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerEmail")] ShoppingBasket shoppingBasket)
        {
            if (id != shoppingBasket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingBasket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingBasketExists(shoppingBasket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerEmail"] = new SelectList(_context.Set<Customer>(), "Id", "Id", shoppingBasket.CustomerId);
            return View(shoppingBasket);
        }
        
       


        // GET: ShoppingBaskets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBasket = await _context.ShoppingBaskets
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBasket == null)
            {
                return NotFound();
            }

            return View(shoppingBasket);
        }
        [HttpPost]
        public async Task<IActionResult> ProceedToCheckout(int basketId)
        {
            var basket = await _context.ShoppingBaskets
                .Include(b => b.ShoppingBasketBooks)
                .ThenInclude(sb => sb.Book)
                .FirstOrDefaultAsync(b => b.Id == basketId);

            if (basket == null)
            {
                return NotFound();
            }

            var order = new Order
            {
                CustomerEmail = basket.CustomerId, // або отримайте email користувача
                OrderDate = DateTime.Now,
                TotalPrice = basket.ShoppingBasketBooks.Sum(sb => sb.Book.Price * sb.Count),
                ShoppingBasketId = basket.Id,
                OrderStatus = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Checkout", "Orders", new { id = order.OrderId });
        }

        // POST: ShoppingBaskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingBasket = await _context.ShoppingBaskets.FindAsync(id);
            if (shoppingBasket != null)
            {
                _context.ShoppingBaskets.Remove(shoppingBasket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingBasketExists(int id)
        {
            return _context.ShoppingBaskets.Any(e => e.Id == id);
        }
    }
}
