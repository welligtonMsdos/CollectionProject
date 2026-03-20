namespace Collection10Api.src.Application.Dtos.Concert;

public record ConcertCreateDto(string Artist,
                               string Venue,
                               DateOnly ShowDate,
                               string Photo
);
