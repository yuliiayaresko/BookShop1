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

namespace BookInfrastructure.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksShopdatabaseContext _context;

        public BooksController(BooksShopdatabaseContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? categoryId)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (categoryId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.CategoryId == categoryId.Value);
            }

            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            return View(await booksQuery.ToListAsync());
        }




        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
           

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,AuthorName,Genre,Year,Description,Price,CategoryId,Id")] Book book, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Обробка завантаження фото
                if (ImageFile != null)
                {
                    // Генерація унікального імені для зображення
                    var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    var extension = Path.GetExtension(ImageFile.FileName);
                    var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

                    // Завантаження файлу
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Зберігаємо шлях до зображення у моделі
                    book.ImagePath = "/images/" + newFileName;
                }

                // Додавання книги до контексту та збереження
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        [Authorize(Roles = "Admin")]
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,AuthorName,Genre,Year,Description,Price,CategoryId,Id,ImagePath")] Book book, IFormFile? ImageFile)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _context.Books.FindAsync(id);

                    if (existingBook == null)
                    {
                        return NotFound();
                    }

                   
                    existingBook.Name = book.Name;
                    existingBook.AuthorName = book.AuthorName;
                    existingBook.Genre = book.Genre;
                    existingBook.Year = book.Year;
                    existingBook.Description = book.Description;
                    existingBook.Price = book.Price;
                    existingBook.CategoryId = book.CategoryId;

                    
                    if (ImageFile != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                        var extension = Path.GetExtension(ImageFile.FileName);
                        var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

                        // Завантаження нового файлу
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        // Видалення старого зображення (якщо воно було)
                        if (!string.IsNullOrEmpty(existingBook.ImagePath))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingBook.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Оновлення шляху до нового зображення
                        existingBook.ImagePath = "/images/" + newFileName;
                    }

                    _context.Update(existingBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }

            return View(book);
        }


        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
