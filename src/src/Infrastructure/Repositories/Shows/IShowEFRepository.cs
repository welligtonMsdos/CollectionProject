using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.Shows;

public interface IShowEFRepository : IRepository
{
    Task<Show> CreateShowAsync(Show show);

    Task<Show> UpdateShowAsync(Show show);

    Task<bool> DeleteShowAsync(Show show);
}
