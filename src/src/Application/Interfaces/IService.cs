namespace Collection10Api.src.Application.Interfaces;

public interface IService<T>
{
    Task<ICollection<T>> GetAllAsync();

    Task<T> GetByGuidAsync(Guid guid);    

    Task<bool> DeleteAsync(Guid guid);
}
