using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Domain.Entities;

namespace Collection10Api.src.Application.Extensions;

public static class ConcertExtensions
{
    public static string ToFormattedDateString(this DateOnly date)
    {
        return date.ToString(
            "dd 'de' MMMM 'de' yyyy",
            new System.Globalization.CultureInfo("pt-BR")
        );
    }

    public static ConcertDto ToConcertDto(this Concert concert)
    {
        ArgumentNullException.ThrowIfNull(concert);

        return new ConcertDto
        (
            concert.Guid,
            concert.Artist,
            concert.Venue,
            concert.ShowDate,
            ToFormattedDateString(concert.ShowDate),
            concert.Photo
        );
    } 

    public static Concert ToEntity(this ConcertCreateDto concertCreateDto)
    {       
        ArgumentNullException.ThrowIfNull(concertCreateDto);

        return new Concert
        {            
            Artist = concertCreateDto.Artist,
            Venue = concertCreateDto.Venue,
            ShowDate = concertCreateDto.ShowDate,
            Photo = concertCreateDto.Photo,
            UserId = string.Empty,
        };
    }

    public static void UpdateEntity(this Concert concert, ConcertUpdateDto concertUpdateDto)
    {
            ArgumentNullException.ThrowIfNull(concert);
            ArgumentNullException.ThrowIfNull(concertUpdateDto);
    
            concert.Artist = concertUpdateDto.Artist;
            concert.Venue = concertUpdateDto.Venue;
            concert.ShowDate = concertUpdateDto.ShowDate;
            concert.Photo = concertUpdateDto.Photo;           
    }
}
