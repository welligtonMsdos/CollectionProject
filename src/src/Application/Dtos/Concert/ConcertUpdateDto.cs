
namespace Collection10Api.src.Application.Dtos.Concert;

public record ConcertUpdateDto(Guid Guid,
                               string Artist,
                               string Venue,
                               DateOnly ShowDate,
                               string Photo)
{};
