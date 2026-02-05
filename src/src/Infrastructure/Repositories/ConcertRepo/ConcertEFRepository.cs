using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Data.Context;

namespace Collection10Api.src.Infrastructure.Repositories.ConcertRepo;

public class ConcertEFRepository : IConcertEFRepository
{
    private readonly CollectionContext _context;

    public ConcertEFRepository(CollectionContext context)
    {
        _context = context;
    }

    public async Task<Concert> CreateAsync(Concert obj)
    {
        await _context.concerts.AddAsync(obj);

        await _context.SaveChangesAsync();

        return obj;
    }

    public async Task<bool> DeleteAsync(Concert obj)
    {
        _context.concerts.Remove(obj);

        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }   

    public async Task<Concert> UpdateAsync(Concert obj)
    {
        _context.concerts.Update(obj);

        await _context.SaveChangesAsync();

        return obj;
    }
}
