﻿using System;
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
            var booksShopdatabaseContext = _context.ShoppingBasketBooks
                .Include(s => s.Book)
                .Include(s => s.ShoppingBasket);
            return View(await booksShopdatabaseContext.ToListAsync());
        }

        // GET: ShoppingBasketBooks/Details
        public async Task<IActionResult> Details(int? shoppingBasketId, int? bookId)
        {
            if (shoppingBasketId == null || bookId == null)
            {
                return NotFound();
            }

            var shoppingBasketBook = await _context.ShoppingBasketBooks
                .Include(s => s.Book)
                .Include(s => s.ShoppingBasket)
                .FirstOrDefaultAsync(m => m.ShoppingBasketId == shoppingBasketId && m.BookId == bookId);
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
            ViewData["ShoppingBasketId"] = new SelectList(_context.ShoppingBaskets, "Id", "CustomerId");
            return View();
        }

        // POST: ShoppingBasketBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShoppingBasketId,BookId,Count")] ShoppingBasketBook shoppingBasketBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingBasketBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "AuthorName", shoppingBasketBook.BookId);
            ViewData["ShoppingBasketId"] = new SelectList(_context.ShoppingBaskets, "Id", "CustomerId", shoppingBasketBook.ShoppingBasketId);
            return View(shoppingBasketBook);
        }

        // GET: ShoppingBasketBooks/Edit
        public async Task<IActionResult> Edit(int? shoppingBasketId, int? bookId)
        {
            if (shoppingBasketId == null || bookId == null)
            {
                return NotFound();
            }

            var shoppingBasketBook = await _context.ShoppingBasketBooks
                .FirstOrDefaultAsync(m => m.ShoppingBasketId == shoppingBasketId && m.BookId == bookId);
            if (shoppingBasketBook == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "AuthorName", shoppingBasketBook.BookId);
            ViewData["ShoppingBasketId"] = new SelectList(_context.ShoppingBaskets, "Id", "CustomerId", shoppingBasketBook.ShoppingBasketId);
            return View(shoppingBasketBook);
        }

        // POST: ShoppingBasketBooks/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int shoppingBasketId, int bookId, [Bind("ShoppingBasketId,BookId,Count")] ShoppingBasketBook shoppingBasketBook)
        {
            if (shoppingBasketId != shoppingBasketBook.ShoppingBasketId || bookId != shoppingBasketBook.BookId)
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
                    if (!ShoppingBasketBookExists(shoppingBasketId, bookId))
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
            ViewData["ShoppingBasketId"] = new SelectList(_context.ShoppingBaskets, "Id", "CustomerId", shoppingBasketBook.ShoppingBasketId);
            return View(shoppingBasketBook);
        }

        // GET: ShoppingBasketBooks/Delete
        public async Task<IActionResult> Delete(int? shoppingBasketId, int? bookId)
        {
            if (shoppingBasketId == null || bookId == null)
            {
                return NotFound();
            }

            var shoppingBasketBook = await _context.ShoppingBasketBooks
                .Include(s => s.Book)
                .Include(s => s.ShoppingBasket)
                .FirstOrDefaultAsync(m => m.ShoppingBasketId == shoppingBasketId && m.BookId == bookId);
            if (shoppingBasketBook == null)
            {
                return NotFound();
            }

            return View(shoppingBasketBook);
        }

        // POST: ShoppingBasketBooks/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int shoppingBasketId, int bookId)
        {
            var shoppingBasketBook = await _context.ShoppingBasketBooks
                .FirstOrDefaultAsync(m => m.ShoppingBasketId == shoppingBasketId && m.BookId == bookId);
            if (shoppingBasketBook != null)
            {
                _context.ShoppingBasketBooks.Remove(shoppingBasketBook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingBasketBookExists(int shoppingBasketId, int bookId)
        {
            return _context.ShoppingBasketBooks.Any(e => e.ShoppingBasketId == shoppingBasketId && e.BookId == bookId);
        }
    }
}