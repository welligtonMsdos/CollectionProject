using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.Vinils;

public interface IVinilDapperRepository: IRepository
{
    Task<ICollection<Vinil>> GetAllVinilsAsync();

    Task<Vinil> GetVinilByIdAsync(int id);
}
