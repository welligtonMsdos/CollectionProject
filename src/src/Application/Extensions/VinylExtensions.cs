using Collection10Api.src.Application.Dtos.Vinyl;
using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Application.Extensions;

public static class VinylExtensions
{
    public static VinylDto ToVinylDto(this Vinyl vinyl)
    {
        ArgumentNullException.ThrowIfNull(vinyl);

        return new VinylDto
        (
            vinyl.Guid,
            vinyl.Artist,
            vinyl.Album,
            vinyl.Year,
            vinyl.Photo,
            vinyl.Price
        );
    }

    public static Vinyl ToEntity(this VinylCreateDto vinylCreateDto)
    {
        ArgumentNullException.ThrowIfNull(vinylCreateDto);
        return new Vinyl
        {
            Artist = vinylCreateDto.Artist,
            Album = vinylCreateDto.Album,
            Year = vinylCreateDto.Year,
            Photo = vinylCreateDto.Photo,
            Price = vinylCreateDto.Price
        };
    }

    public static void UpdateEntity(this Vinyl vinyl, VinylUpdateDto vinylUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(vinyl);
        ArgumentNullException.ThrowIfNull(vinylUpdateDto);

        vinyl.Artist = vinylUpdateDto.Artist;
        vinyl.Album = vinylUpdateDto.Album;
        vinyl.Year = vinylUpdateDto.Year;
        vinyl.Photo = vinylUpdateDto.Photo;
        vinyl.Price = vinylUpdateDto.Price;
    }
}
