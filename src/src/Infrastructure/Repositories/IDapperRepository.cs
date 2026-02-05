namespace Collection10Api.src.Infrastructure.Repositories;

public interface IDapperRepository<T>
{
    Task<T?> GetByGuidAsync(Guid guid);
    Task<IEnumerable<T>> GetAllAsync();
}
