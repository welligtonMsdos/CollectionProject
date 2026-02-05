namespace Collection10Api.src.Application.Dtos.Vinyl;

public record VinylUpdateDto(Guid Guid,
                             string Artist,
                             string Album,
                             int Year,
                             string Photo,
                             decimal Price)
{}
