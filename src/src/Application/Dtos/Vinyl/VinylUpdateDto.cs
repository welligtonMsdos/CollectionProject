namespace Collection10Api.src.Application.Dtos.Vinyl;

public record VinylUpdateDto(string Artist,
                             string Album,
                             int Year,
                             string Photo,
                             decimal Price)
{}
