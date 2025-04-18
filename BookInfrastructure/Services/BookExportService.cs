using BookDomain.Model;
using BookInfrastructure;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace BookInfrastructure.Services
{
    public class BookExportService : IExportService<Book>
    {
        private readonly BooksShopdatabaseContext _context;
        private static readonly string[] HeaderNames = { "Назва", "Автор", "Жанр", "Рік видання", "Опис", "Ціна" };

        public BookExportService(BooksShopdatabaseContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Потік не доступний для запису");
            }

            // Завантажуємо всі книги разом із їхніми категоріями
            var books = await _context.Books
                .Include(b => b.Categories) 
                .ToListAsync(cancellationToken);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Книги");
                WriteHeader(worksheet);
                WriteBooks(worksheet, books);
                workbook.SaveAs(stream);
            }
        }

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int i = 0; i < HeaderNames.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = HeaderNames[i];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private static void WriteBooks(IXLWorksheet worksheet, IList<Book> books)
        {
            int rowIndex = 2;
            foreach (var book in books)
            {
                worksheet.Cell(rowIndex, 1).Value = book.Name;
                worksheet.Cell(rowIndex, 2).Value = book.AuthorName;
                worksheet.Cell(rowIndex, 3).Value = book.Genre;
                worksheet.Cell(rowIndex, 4).Value = book.Year;
                worksheet.Cell(rowIndex, 5).Value = book.Description;
                worksheet.Cell(rowIndex, 6).Value = book.Price;

                string categoryNames = book.Categories.Any()
                    ? string.Join(", ", book.Categories.Select(c => c.CategoryName))
                    : "Без категорії";
                worksheet.Cell(rowIndex, 7).Value = categoryNames;

                // Діагностика
                Console.WriteLine($"Book ID: {book.Id}, Categories: {categoryNames}");
                rowIndex++;
            }
        }
    }
}