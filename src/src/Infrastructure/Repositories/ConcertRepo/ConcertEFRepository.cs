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
    public async Task<Concert> CreateConcertAsync(Concert concert)
    {
        await _context.concerts.AddAsync(concert);

        await _context.SaveChangesAsync();

        return concert;
    }

    public async Task<bool> DeleteConcertAsync(Concert concert)
    {
        _context.concerts.Remove(concert);

        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }

    public async Task<Concert> UpdateConcertAsync(Concert concert)
    {
        _context.concerts.Update(concert);

        await _context.SaveChangesAsync();

        return concert;
    }
}
