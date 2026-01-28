
using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.Vinils;

public interface IVinilEFRepository : IRepository
{
    Task<Vinil> CreateVinilAsync(Vinil vinil);

    Task<Vinil> UpdateVinilAsync(Vinil vinil);

    Task<bool> DeleteVinilAsync(Vinil vinil);
}
