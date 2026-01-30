namespace Collection10Api.src.Application.Dtos.Concert;

public record ConcertDto(Guid Guid,
                         string Artist,
                         string Venue,
                         string ShowDateDescription,
                         string Photo) { };
