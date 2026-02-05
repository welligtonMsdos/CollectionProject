using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.VinylRepo;

public class VinylDapperRepository : BaseRepository, IVinylDapperRepository
{   
    public VinylDapperRepository(IConfiguration config): base(config) { }

    public async Task<IEnumerable<Vinyl>> GetAllAsync()
    {
        return await GetAllAsync<Vinyl>();
    }

    public async Task<Vinyl?> GetByGuidAsync(Guid guid)
    {
        return await GetByIdAsync<Vinyl>(guid);
    }
}
