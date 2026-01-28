namespace Collection10Api.src.Application.Dtos.Vinil;

public record VinilCreateDto(string Artist,
                             string Album,
                             int Year,
                             string Photo,
                             decimal Price){}

