using Microsoft.AspNetCore.Mvc;
using BookDomain.Model;
using BookInfrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookInfrastructure.Controllers
{
    public class HomeController : Controller
    {
        private readonly BooksShopdatabaseContext _context;

        public HomeController(BooksShopdatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? genre)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(genre))
            {
                booksQuery = booksQuery.Where(b => b.Genre == genre);
            }

            var genres = await _context.Books
                .Select(b => b.Genre)
                .Distinct()
                .ToListAsync();

            ViewBag.Genres = new SelectList(genres);
            ViewBag.Categories = _context.Categories.ToList();

            return View(await booksQuery.ToListAsync());
        }
    }

}
