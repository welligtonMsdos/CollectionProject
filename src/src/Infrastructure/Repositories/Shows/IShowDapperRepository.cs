using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.Shows;

public interface IShowDapperRepository : IRepository
{
    Task<ICollection<Show>> GetAllShowsAsync();

    Task<ICollection<Show>> GetAllShowsUpcomingAsync();

    Task<ICollection<Show>> GetAllShowsPastAsync();

    Task<Show> GetShowByGuidAsync(Guid guid);
}
