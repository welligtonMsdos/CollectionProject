using AutoMapper;
using Collection10Api.src.Application.Dtos.Show;
using Collection10Api.src.Application.Dtos.Vinil;
using Collection10Api.src.Domain.Entities;
using System.Globalization;

namespace Collection10Api.src.Infrastructure.Profiles;

public class CollectionProfile : Profile
{
    public CollectionProfile()
    {
        CreateMap<Vinil, VinilDto>().ReverseMap();
        CreateMap<Vinil, VinilCreateDto>().ReverseMap();
        CreateMap<Vinil, VinilUpdateDto>().ReverseMap();

        CreateMap<Show, ShowDto>()
            .ConstructUsing(src => new ShowDto(
                            src.Guid,
                            src.Artist,
                            src.Venue,
                            src.ShowDate.ToString(
                                "dd 'de' MMMM 'de' yyyy",
                                new CultureInfo("pt-BR")
                            ),
                            src.Photo
            ));

        CreateMap<Show, ShowCreateDto>().ReverseMap();
        CreateMap<Show, ShowUpdateDto>().ReverseMap();
    }
}
