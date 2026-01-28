using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Data.Context;

namespace Collection10Api.src.Infrastructure.Repositories.VinylRepo;

public class VinylEFRepository : IVinylEFRepository
{
    private readonly CollectionContext _context;

    public VinylEFRepository(CollectionContext context)
    {
        _context = context;
    }

    public async Task<Vinyl> CreateVinylAsync(Vinyl vinyl)
    {
        await _context.vinyls.AddAsync(vinyl);

        await _context.SaveChangesAsync();

        return vinyl;
    }

    public async Task<bool> DeleteVinylAsync(Vinyl vinyl)
    {
        _context.vinyls.Remove(vinyl);

        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }

    public async Task<Vinyl> UpdateVinylAsync(Vinyl vinyl)
    {
        _context.vinyls.Update(vinyl);

        await _context.SaveChangesAsync();

        return vinyl;
    }
}
