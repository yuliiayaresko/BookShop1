
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookInfrastructure;
using BookDomain.Model;
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
            var booksShopdatabaseContext = _context.ShoppingBaskets.Include(s => s.CustomerEmailNavigation);
            return View(await booksShopdatabaseContext.ToListAsync());
        }

        public async Task<IActionResult> AddBook(int basketId)
        {
            // Завантажуємо кошик разом із зв’язаними книгами (якщо потрібно)
            var basket = await _context.ShoppingBaskets
                .Include(b => b.ShoppingBasketBooks) // Додаємо, якщо потрібно відображати книги в кошику
                .FirstOrDefaultAsync(b => b.Id == basketId);

            if (basket == null) return NotFound();


            // Передаємо список усіх книг через ViewBag
            ViewBag.Books = await _context.Books.ToListAsync();

            // Повертаємо сам кошик як модель представлення
            return View(basket);
        }



        [HttpPost]
        public async Task<IActionResult> AddToBasket(int basketId, int Id, int count)
        {
            if (count <= 0)
            {
                return BadRequest("Кількість книг має бути більше 0.");
            }

            // Перевіряємо, чи існує така книга в базі
            var bookExists = await _context.Books.AnyAsync(b => b.Id == Id);
            if (!bookExists)
            {
                return NotFound($"Книга з ID {Id} не знайдена.");
            }

            // Перевіряємо, чи існує кошик
            var basket = await _context.ShoppingBaskets
                .Include(b => b.ShoppingBasketBooks)
                .FirstOrDefaultAsync(b => b.Id == basketId);
            if (basket == null)
            {
                return NotFound($"Кошик з ID {basketId} не знайдений.");
            }

            // Перевіряємо, чи ця книга вже є в кошику
            var existingItem = basket.ShoppingBasketBooks?.FirstOrDefault(sb => sb.Id == Id);

            if (existingItem != null)
            {
                existingItem.Count += count;
            }
            else
            {
                var newItem = new ShoppingBasketBook
                {
                    ShoppingBasketId = basketId,
                    Id = Id,
                    Count = count
                };

                _context.ShoppingBasketBooks.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = basketId });
        }




        // Деталі кошика з книгами
        public async Task<IActionResult> Details(int id)
        {
            var basket = await _context.ShoppingBaskets
     .Include(b => b.ShoppingBasketBooks)
     .ThenInclude(sbb => sbb.Book)
     .FirstOrDefaultAsync(b => b.Id == id);

            if (basket == null)
            {
                return NotFound();
            }

            return View(basket);
        }




        // GET: ShoppingBaskets/Create
        public IActionResult Create()
        {
            ViewData["CustomerEmail"] = new SelectList(_context.Customers, "Email", "Email");
            return View();
        }

        // POST: ShoppingBaskets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

       
public async Task<IActionResult> Create([Bind("CustomerEmail,Id")] ShoppingBasket shoppingBasket)
        {
            // Перевірка, чи існує користувач із таким email
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == shoppingBasket.CustomerEmail);

            if (customer == null)
            {
                return BadRequest("Customer not found.");
            }

            // Призначаємо навігаційну властивість
            shoppingBasket.CustomerEmailNavigation = customer;



            // Додаємо новий кошик у базу
            _context.Add(shoppingBasket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
            ViewData["CustomerEmail"] = new SelectList(_context.Customers, "Email", "Email", shoppingBasket.CustomerEmail);
            return View(shoppingBasket);
        }

        // POST: ShoppingBaskets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShoppingBasket shoppingBasket)
        {
            if (id != shoppingBasket.Id)
            {
                return NotFound();
            }

            // Отримуємо існуючий запис з бази
            var existingBasket = await _context.ShoppingBaskets.FindAsync(id);
            if (existingBasket == null)
            {
                return NotFound();
            }

            try
            {
                // Оновлюємо тільки змінні поля (без Id)
                existingBasket.CustomerEmail = shoppingBasket.CustomerEmail;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingBasketExists(id))
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


        // GET: ShoppingBaskets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBasket = await _context.ShoppingBaskets
                .Include(s => s.CustomerEmailNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBasket == null)
            {
                return NotFound();
            }

            return View(shoppingBasket);
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