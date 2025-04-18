using BookDomain.Model;
using BookInfrastructure;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookInfrastructure.Services
{
    public class BookImportService : IImportService<Book>
    {
        private readonly BooksShopdatabaseContext _context;
        private readonly ILogger<BookImportService> _logger;

        public BookImportService(BooksShopdatabaseContext context, ILogger<BookImportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Потік не доступний для читання", nameof(stream));
            }

            using (var workbook = new XLWorkbook(stream))
            {
                var existingCategories = await _context.Categories.ToListAsync(cancellationToken);

                foreach (var worksheet in workbook.Worksheets)
                {
                    _logger.LogInformation("Обробка аркуша: {WorksheetName}", worksheet.Name);

                    foreach (var row in worksheet.RowsUsed().Skip(1)) // Пропускаємо заголовок
                    {
                        var name = row.Cell(1).GetString().Trim();
                        var authorName = row.Cell(2).GetString().Trim();
                        var genre = row.Cell(3).GetString().Trim();
                        var year = row.Cell(4).GetValue<int>();
                        var description = row.Cell(5).GetString().Trim();
                        var price = row.Cell(6).GetValue<decimal>();
                        var categoryName = row.Cell(7).GetString().Trim();
                        var imagePath = row.Cell(8).GetString().Trim();

                        _logger.LogInformation("Обробка книги: {BookName}, Категорія: {CategoryName}, Шлях до зображення: {ImagePath}", name, categoryName, imagePath);

                        var existingCategory = existingCategories
                            .FirstOrDefault(c => c.CategoryName.Trim().ToLower() == categoryName.Trim().ToLower());

                        if (existingCategory == null)
                        {
                            _logger.LogError("Категорія '{CategoryName}' не знайдена. Книга '{BookName}' не імпортована.", categoryName, name);
                            continue;
                        }

                        await AddOrUpdateBookAsync(name, authorName, genre, year, description, price, existingCategory, imagePath, cancellationToken);
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task AddOrUpdateBookAsync(string name, string authorName, string genre, int year, string description, decimal price, Category category, string imagePath, CancellationToken cancellationToken)
        {
            // Шукаємо книгу за назвою та автором
            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Name == name && b.AuthorName == authorName, cancellationToken);

            if (existingBook == null)
            {
                // Якщо книги немає, додаємо нову
                var newBook = new Book
                {
                    Name = name,
                    AuthorName = authorName,
                    Genre = genre,
                    Year = year,
                    Description = description,
                    Price = price,
                    CategoryId = category.Id,
                    ImagePath = string.IsNullOrWhiteSpace(imagePath) ? null : imagePath
                };
                _context.Books.Add(newBook);
                _logger.LogInformation("Додано нову книгу: {BookName} до категорії {CategoryName} з ImagePath: {ImagePath}", newBook.Name, category.CategoryName, newBook.ImagePath);
            }
            else
            {
                // Якщо книга існує, оновлюємо її властивості
                if (existingBook.CategoryId != category.Id || existingBook.ImagePath != imagePath ||
                    existingBook.Genre != genre || existingBook.Year != year || existingBook.Description != description || existingBook.Price != price)
                {
                    existingBook.Genre = genre;
                    existingBook.Year = year;
                    existingBook.Description = description;
                    existingBook.Price = price;
                    existingBook.CategoryId = category.Id;
                    existingBook.ImagePath = string.IsNullOrWhiteSpace(imagePath) ? null : imagePath;

                    // Оновлюємо сутність у контексті
                    _context.Entry(existingBook).State = EntityState.Modified;
                    _logger.LogInformation("Оновлено книгу: {BookName} - CategoryId: {CategoryId}, ImagePath: {ImagePath}", existingBook.Name, category.Id, existingBook.ImagePath);
                }
                else
                {
                    _logger.LogInformation("Книга {BookName} вже існує з ідентичними даними, оновлення не потрібне.", existingBook.Name);
                }
            }
        }
    }
}