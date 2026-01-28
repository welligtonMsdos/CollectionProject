namespace Collection10Api.src.Application.Dtos.Vinyl;

public record VinylCreateDto(string Artist,
                             string Album,
                             int Year,
                             string Photo,
                             decimal Price){}

