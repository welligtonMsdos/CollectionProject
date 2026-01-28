namespace Collection10Api.src.Application.Dtos.Show;

public record ShowCreateDto(string Artist,
                            string Venue,
                            DateOnly ShowDate,
                            string Photo)
{};
