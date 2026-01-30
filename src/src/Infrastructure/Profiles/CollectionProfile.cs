using AutoMapper;
using Collection10Api.src.Application.Dtos.Concert;
using Collection10Api.src.Application.Dtos.Vinyl;
using Collection10Api.src.Domain.Entities;
using System.Globalization;

namespace Collection10Api.src.Infrastructure.Profiles;

public class CollectionProfile : Profile
{
    public CollectionProfile()
    {
        CreateMap<Vinyl, VinylDto>().ReverseMap();
        CreateMap<Vinyl, VinylCreateDto>().ReverseMap();
        CreateMap<Vinyl, VinylUpdateDto>().ReverseMap();

        CreateMap<Concert, ConcertDto>()
            .ConstructUsing(src => new ConcertDto(
                            src.Guid,
                            src.Artist,
                            src.Venue,
                            src.ShowDate.ToString(
                                "dd 'de' MMMM 'de' yyyy",
                                new CultureInfo("pt-BR")
                            ),
                            src.Photo
            ));

        CreateMap<Concert, ConcertCreateDto>().ReverseMap();
        CreateMap<Concert, ConcertUpdateDto>().ReverseMap();
    }
}
