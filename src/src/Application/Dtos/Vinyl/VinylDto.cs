namespace Collection10Api.src.Application.Dtos.Vinyl;

public record VinylDto(int Id, 
                       string Artist, 
                       string Album,
                       int Year,
                       string Photo,
                       decimal Price) { }
