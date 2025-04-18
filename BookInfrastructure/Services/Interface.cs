using BookDomain.Model;

namespace BookInfrastructure.Services
{
    public interface IImportService<TEntity> where TEntity : Entity
    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }

    public interface IExportService<TEntity> where TEntity : Entity
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }

    public interface IDataPortServiceFactory<TEntity> where TEntity : Entity
    {
        IImportService<TEntity> GetImportService(string contentType);
        IExportService<TEntity> GetExportService(string contentType);
    }
}