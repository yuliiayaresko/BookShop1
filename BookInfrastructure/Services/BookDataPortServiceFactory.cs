using BookDomain.Model;
using BookInfrastructure;
using Microsoft.Extensions.Logging;

namespace BookInfrastructure.Services
{
    public class BookDataPortServiceFactory : IDataPortServiceFactory<Book>
    {
        private readonly BooksShopdatabaseContext _context;
        private readonly ILogger<BookImportService> _logger; // Додаємо ILogger

        public BookDataPortServiceFactory(BooksShopdatabaseContext context, ILogger<BookImportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IImportService<Book> GetImportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new BookImportService(_context, _logger); // Передаємо logger
            }
            throw new NotImplementedException($"Імпорт для типу {contentType} не підтримується.");
        }

        public IExportService<Book> GetExportService(string contentType)
        {
            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new BookExportService(_context);
            }
            throw new NotImplementedException($"Експорт для типу {contentType} не підтримується.");
        }
    }
}