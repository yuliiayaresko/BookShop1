using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookDomain.Model;

namespace BookInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly BooksShopdatabaseContext _context;

        public ChartsController(BooksShopdatabaseContext context)
        {
            _context = context;
        }

        // Діаграма 1: Які книги купували та їх кількість
        [HttpGet("booksPurchased")]
        public async Task<JsonResult> GetBooksPurchasedAsync(CancellationToken cancellationToken)
        {
            var booksPurchased = await _context.OrderDetails
                .Include(od => od.Book)
                .GroupBy(od => od.Book.Name)
                .Select(g => new
                {
                    BookName = g.Key,
                    Quantity = g.Sum(od => od.Quantity)
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(booksPurchased);
        }

        // Діаграма 2: Кількість покупок за місяць
        [HttpGet("ordersByMonth")]
        public async Task<JsonResult> GetOrdersByMonthAsync(CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Where(o => o.OrderDate.HasValue)
                .ToListAsync(cancellationToken); // Завантажуємо всі потрібні дані в пам'ять

            var ordersByMonth = orders
                .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    OrderCount = g.Count()
                })
                .OrderBy(g => g.Month) // Сортування вже в пам'яті
                .ToList();

            return new JsonResult(ordersByMonth);
        }


    }
}



