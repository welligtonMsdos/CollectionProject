using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Infrastructure.Repositories.ConcertRepo;

public interface IConcertDapperRepository : IDapperRepository<Concert>
{    
    Task<ICollection<Concert>> GetUpcomingAsync(string userId);

    Task<ICollection<Concert>> GetPastAsync(string userId);
}
