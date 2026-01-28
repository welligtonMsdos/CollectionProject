using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Data.Context;

namespace Collection10Api.src.Infrastructure.Repositories.Vinils;

public class VinilEFRepository : IVinilEFRepository
{
    private readonly CollectionContext _context;

    public VinilEFRepository(CollectionContext context)
    {
        _context = context;
    }

    public async Task<Vinyl> CreateVinilAsync(Vinyl vinil)
    {
        await _context.vinils.AddAsync(vinil);

        await _context.SaveChangesAsync();

        return vinil;
    }

    public async Task<bool> DeleteVinilAsync(Vinyl vinil)
    {
        _context.vinils.Remove(vinil);

        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }

    public async Task<Vinyl> UpdateVinilAsync(Vinyl vinil)
    {
        _context.vinils.Update(vinil);

        await _context.SaveChangesAsync();

        return vinil;
    }
}
