using Collection10Api.src.Application.Dtos.Concert;

namespace Collection10Api.src.Application.Interfaces;

public interface IConcertService : IService
{
    Task<ICollection<ConcertDto>> GetAllConcertsAsync();

    Task<ICollection<ConcertDto>> GetAllConcertsUpcomingAsync();

    Task<ICollection<ConcertDto>> GetAllConcertsPastAsync();

    Task<ConcertDto> GetConcertByGuidAsync(Guid guid);

    Task<ConcertDto> CreateConcertAsync(ConcertCreateDto concertCreateDto);

    Task<ConcertDto> UpdateConcertAsync(ConcertUpdateDto concertUpdateDto);

    Task<bool> DeleteConcertAsync(Guid guid);
}
