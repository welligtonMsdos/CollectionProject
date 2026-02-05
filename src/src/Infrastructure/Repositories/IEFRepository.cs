namespace Collection10Api.src.Infrastructure.Repositories;

public interface IEFRepository<T>
{
    Task<T> CreateAsync(T obj);

    Task<T> UpdateAsync(T obj);

    Task<bool> DeleteAsync(T obj);
}
