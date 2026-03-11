namespace Collection10Api.src.Infrastructure.Repositories;

public interface IEFRepository<T>
{
    Task<T> PostAsync(T obj);

    Task<T> PutAsync(T obj);

    Task<bool> DeleteAsync(T obj);
}
