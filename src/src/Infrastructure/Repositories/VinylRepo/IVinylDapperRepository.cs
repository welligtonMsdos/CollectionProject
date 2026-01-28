using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.VinylRepo;

public interface IVinylDapperRepository: IRepository
{
    Task<ICollection<Vinyl>> GetAllVinylsAsync();

    Task<Vinyl> GetVinylByIdAsync(int id);
}
