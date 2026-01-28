
using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.Vinils;

public interface IVinilEFRepository : IRepository
{
    Task<Vinyl> CreateVinilAsync(Vinyl vinil);

    Task<Vinyl> UpdateVinilAsync(Vinyl vinil);

    Task<bool> DeleteVinilAsync(Vinyl vinil);
}
