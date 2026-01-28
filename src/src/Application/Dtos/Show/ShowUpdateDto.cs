
namespace Collection10Api.src.Application.Dtos.Show;

public record ShowUpdateDto(Guid Guid,
                            string Artist,
                            string Venue,
                            DateOnly ShowDate,
                            string Photo)
{};
