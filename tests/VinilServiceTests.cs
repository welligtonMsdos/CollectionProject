using AutoMapper;
using Collection10Api.src.Application.Dtos.Vinil;
using Collection10Api.src.Application.Services;
using Collection10Api.src.Domain.Entities;
using Collection10Api.src.Infrastructure.Profiles;
using Collection10Api.src.Infrastructure.Repositories.Vinils;
using FluentAssertions;
using Moq;

namespace Collection10.Tests;

public class VinilServiceTests
{
    private readonly Mock<IVinilDapperRepository> _dapperRepositoy;
    private readonly Mock<IVinilEFRepository> _efRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly VinilService _service;

    public VinilServiceTests()
    {
        _dapperRepositoy = new Mock<IVinilDapperRepository>();

        _efRepository = new Mock<IVinilEFRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CollectionProfile>();
        });

        var mapper = mapperConfig.CreateMapper();

        _service = new VinilService(_dapperRepositoy.Object, _efRepository.Object, mapper);
    }


    [Fact]
    public async Task CreateAsyn_ShouldAddVinyl()
    {
        var dto = new VinilCreateDto("Pink Floyd",
                                     "The Wall",
                                     1979,
                                     "https://example.com/thewall.jpg",
                                     200);

        await _service.CreateVinilAsync(dto);

        _efRepository.Verify(
            r => r.CreateVinilAsync(It.Is<Vinil>(v =>
                v.Artist == dto.Artist &&
                v.Album == dto.Album &&
                v.Year == dto.Year &&
                v.Photo == dto.Photo &&
                v.Price == dto.Price
            )),
            Times.Once
            );
    }

    //[Fact]
    //public async Task CreateAsyn_ShouldThrow() 
    //{   
    //    var dto = new VinilCreateDto("", 
    //                                 "The Wall", 
    //                                 1979, 
    //                                 "https://example.com/thewall.jpg", 
    //                                 200);

    //    await _service.CreateVinilAsync(dto);

    //    _efRepository.Verify(
    //        r => r.CreateVinilAsync(It.IsAny<Vinil>()),
    //        Times.Never
    //        );
    //}

    [Fact]
    public async Task GetAllVinilsAsync_ShouldReturnVinyls()
    {
        var vinils = new List<VinilDto>
        {
            new VinilDto(1, "Pink Floyd", "The Wall", 1979, "https://example.com/thewall.jpg", 200),
            new VinilDto(2, "The Beatles", "Abbey Road", 1969, "https://example.com/abbeyroad.jpg", 150)
        };

        _dapperRepositoy.Setup(r => r.GetAllVinilsAsync())
                        .ReturnsAsync(vinils.Select(v => new Vinil
                        {
                            Id = v.Id,
                            Artist = v.Artist,
                            Album = v.Album,
                            Year = v.Year,
                            Photo = v.Photo,
                            Price = v.Price
                        }).ToList());

        var result = await _service.GetAllVinilsAsync();

        result.Should().BeEquivalentTo(vinils);

        _dapperRepositoy.Verify(r => r.GetAllVinilsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetVinilByIdAsync_ShouldReturnVinyl()
    {
        var vinil = new VinilDto(1,
                                 "Pink Floyd",
                                 "The Wall",
                                 1979,
                                 "https://example.com/thewall.jpg",
                                 200);

        _dapperRepositoy.Setup(r => r.GetVinilByIdAsync(vinil.Id))
                        .ReturnsAsync(new Vinil
                        {
                            Id = vinil.Id,
                            Artist = vinil.Artist,
                            Album = vinil.Album,
                            Year = vinil.Year,
                            Photo = vinil.Photo,
                            Price = vinil.Price
                        });

        var result = await _service.GetVinilByIdAsync(vinil.Id);

        result.Should().BeEquivalentTo(vinil);

        _dapperRepositoy.Verify(r => r.GetVinilByIdAsync(vinil.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteVinilAsync_ShouldDeleteVinyl()
    {
        var vinilId = 1;

        _dapperRepositoy.Setup(r => r.GetVinilByIdAsync(vinilId))
                        .ReturnsAsync(new Vinil
                        {
                            Id = vinilId,
                            Artist = "Pink Floyd",
                            Album = "The Wall",
                            Year = 1979,
                            Photo = "https://example.com/thewall.jpg",
                            Price = 200
                        });

        _efRepository.Setup(r => r.DeleteVinilAsync(It.IsAny<Vinil>()))
                     .ReturnsAsync(true);

        var result = await _service.DeleteVinilAsync(vinilId);

        result.Should().BeTrue();

        _dapperRepositoy.Verify(r => r.GetVinilByIdAsync(vinilId), Times.Once);

        _efRepository.Verify(r => r.DeleteVinilAsync(It.IsAny<Vinil>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsyn_ShouldUpdateVinyl()
    {
        var dto = new VinilUpdateDto(1,
                                     "Pink Floyd",
                                     "The Wall",
                                     1979,
                                     "https://example.com/thewall.jpg",
                                     250);

        await _service.UpdateVinilAsync(dto);

        _efRepository.Verify(
            r => r.UpdateVinilAsync(It.Is<Vinil>(v =>
                v.Id == dto.Id &&
                v.Artist == dto.Artist &&
                v.Album == dto.Album &&
                v.Year == dto.Year &&
                v.Photo == dto.Photo &&
                v.Price == dto.Price
            )),
            Times.Once
            );
    }
}
