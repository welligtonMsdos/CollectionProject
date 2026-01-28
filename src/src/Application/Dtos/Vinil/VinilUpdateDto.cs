namespace Collection10Api.src.Application.Dtos.Vinil;

public record VinilUpdateDto(int Id,
                             string Artist,
                             string Album,
                             int Year,
                             string Photo,
                             decimal Price)
{}
