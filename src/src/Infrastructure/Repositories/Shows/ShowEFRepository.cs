using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Data.Context;

namespace Collection10Api.src.Infrastructure.Repositories.Shows;

public class ShowEFRepository : IShowEFRepository
{
    private readonly CollectionContext _context;

    public ShowEFRepository(CollectionContext context)
    {
        _context = context;
    }
    public async Task<Show> CreateShowAsync(Show show)
    {
        await _context.shows.AddAsync(show);

        await _context.SaveChangesAsync();

        return show;
    }

    public async Task<bool> DeleteShowAsync(Show show)
    {
        _context.shows.Remove(show);

        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }

    public async Task<Show> UpdateShowAsync(Show show)
    {
        _context.shows.Update(show);

        await _context.SaveChangesAsync();

        return show;
    }
}
