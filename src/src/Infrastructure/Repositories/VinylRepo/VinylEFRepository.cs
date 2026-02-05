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

    public async Task<Vinyl> CreateAsync(Vinyl obj)
    {
        await _context.vinyls.AddAsync(obj);

        await _context.SaveChangesAsync();

        return obj;
    }

    public async Task<bool> DeleteAsync(Vinyl obj)
    {
        _context.vinyls.Remove(obj);

        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }

    public async Task<Vinyl> UpdateAsync(Vinyl obj)
    {
        _context.vinyls.Update(obj);

        await _context.SaveChangesAsync();

        return obj;
    }
}
