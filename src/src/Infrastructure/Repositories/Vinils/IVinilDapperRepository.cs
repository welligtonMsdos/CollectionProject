using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.Vinils;

public interface IVinilDapperRepository: IRepository
{
    Task<ICollection<Vinyl>> GetAllVinilsAsync();

    Task<Vinyl> GetVinilByIdAsync(int id);
}
