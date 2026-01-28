namespace Collection10Api.src.Application.Dtos.Vinyl;

public record VinylUpdateDto(int Id,
                             string Artist,
                             string Album,
                             int Year,
                             string Photo,
                             decimal Price)
{}
