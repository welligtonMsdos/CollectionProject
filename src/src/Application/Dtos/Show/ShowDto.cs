namespace Collection10Api.src.Application.Dtos.Show;

public record ShowDto(Guid Guid,
                      string Artist,
                      string Venue,
                      string ShowDateDescription,
                      string Photo) { };
