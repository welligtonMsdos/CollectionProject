using Collection10Api.src.Application.Dtos.Show;

namespace Collection10Api.src.Application.Interfaces;

public interface IShowService : IService
{
    Task<ICollection<ShowDto>> GetAllShowsAsync();

    Task<ICollection<ShowDto>> GetAllShowsUpcomingAsync();

    Task<ICollection<ShowDto>> GetAllShowsPastAsync();

    Task<ShowDto> GetShowByGuidAsync(Guid guid);

    Task<ShowDto> CreateShowAsync(ShowCreateDto showCreateDto);

    Task<ShowDto> UpdateShowAsync(ShowUpdateDto showUpdateDto);

    Task<bool> DeleteShowAsync(Guid guid);
}
