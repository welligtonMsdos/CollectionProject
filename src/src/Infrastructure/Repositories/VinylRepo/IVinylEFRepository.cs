
using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.VinylRepo;

public interface IVinylEFRepository : IRepository
{
    Task<Vinyl> CreateVinylAsync(Vinyl vinyl);

    Task<Vinyl> UpdateVinylAsync(Vinyl vinyl);

    Task<bool> DeleteVinylAsync(Vinyl vinyl);
}
