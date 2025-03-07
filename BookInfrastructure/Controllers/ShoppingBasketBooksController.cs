using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookDomain.Model;
using BookInfrastructure;

namespace BookInfrastructure.Controllers
{
    public class ShoppingBasketBooksController : Controller
    {
        private readonly BooksShopdatabaseContext _context;

        public ShoppingBasketBooksController(BooksShopdatabaseContext context)
        {
            _context = context;
        }

        // GET: ShoppingBasketBooks
        public async Task<IActionResult> Index()
        {
            var booksShopdatabaseContext = _context.ShoppingBasketBooks.Include(s => s.Book).Include(s => s.ShoppingBasket);
            return View(await booksShopdatabaseContext.ToListAsync());
        }

        // GET: ShoppingBasketBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBasketBook = await _context.ShoppingBasketBooks
                .Include(s => s.Book)
                .Include(s => s.ShoppingBasket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBasketBook == null)
            {
                return NotFound();
            }

            return View(shoppingBasketBook);
        }

        // GET: ShoppingBasketBooks/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "AuthorName");
            ViewData["ShoppingBasketId"] = new SelectList(_context.ShoppingBaskets, "Id", "CustomerEmail");
            return View();
        }

        // POST: ShoppingBasketBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShoppingBasketId,BookId,Count")] ShoppingBasketBook shoppingBasketBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingBasketBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "AuthorName", shoppingBasketBook.BookId);
            ViewData["ShoppingBasketId"] = new SelectList(_context.ShoppingBaskets, "Id", "CustomerEmail", shoppingBasketBook.ShoppingBasketId);
            return View(shoppingBasketBook);
        }

        // GET: ShoppingBasketBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBasketBook = await _context.ShoppingBasketBooks.FindAsync(id);
            if (shoppingBasketBook == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "AuthorName", shoppingBasketBook.BookId);
            ViewData["ShoppingBasketId"] = new SelectList(_context.ShoppingBaskets, "Id", "CustomerEmail", shoppingBasketBook.ShoppingBasketId);
            return View(shoppingBasketBook);
        }

        // POST: ShoppingBasketBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShoppingBasketId,BookId,Count")] ShoppingBasketBook shoppingBasketBook)
        {
            if (id != shoppingBasketBook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingBasketBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingBasketBookExists(shoppingBasketBook.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "AuthorName", shoppingBasketBook.BookId);
            ViewData["ShoppingBasketId"] = new SelectList(_context.ShoppingBaskets, "Id", "CustomerEmail", shoppingBasketBook.ShoppingBasketId);
            return View(shoppingBasketBook);
        }

        // GET: ShoppingBasketBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingBasketBook = await _context.ShoppingBasketBooks
                .Include(s => s.Book)
                .Include(s => s.ShoppingBasket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingBasketBook == null)
            {
                return NotFound();
            }

            return View(shoppingBasketBook);
        }

        // POST: ShoppingBasketBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingBasketBook = await _context.ShoppingBasketBooks.FindAsync(id);
            if (shoppingBasketBook != null)
            {
                _context.ShoppingBasketBooks.Remove(shoppingBasketBook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingBasketBookExists(int id)
        {
            return _context.ShoppingBasketBooks.Any(e => e.Id == id);
        }
    }
}
