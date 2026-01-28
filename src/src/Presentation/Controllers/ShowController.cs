using Collection10Api.src.Application.Common;
using Collection10Api.src.Application.Dtos.Show;
using Collection10Api.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collection10Api.src.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowController : Controller
{
    private readonly IShowService _service;

    public ShowController(IShowService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("[Action]")]
    public async Task<IActionResult> CreateShow([FromBody] ShowCreateDto showCreateDto)
    {
        // O ValidationFilter já validou o DTO aqui
        var result = await _service.CreateShowAsync(showCreateDto);

        return CreatedAtAction(nameof(GetShowByGuid), new { guid = result.Guid }, Result<ShowDto>.Ok(result, "Show criado com sucesso!"));
    }

    [Authorize]
    [HttpGet("[Action]")]
    public async Task<IActionResult> GetAllShows()
    {
        var shows = await _service.GetAllShowsAsync();

        return Ok(Result<IEnumerable<ShowDto>>.Ok(shows));
    }

    [Authorize]
    [HttpGet("[Action]")]
    public async Task<IActionResult> GetAllShowsUpcomingAsync()
    {
        var shows = await _service.GetAllShowsUpcomingAsync();

        return Ok(Result<IEnumerable<ShowDto>>.Ok(shows));
    }

    [Authorize]
    [HttpGet("[Action]")]
    public async Task<IActionResult> GetAllShowsPastAsync()
    {
        var shows = await _service.GetAllShowsPastAsync();

        return Ok(Result<IEnumerable<ShowDto>>.Ok(shows));
    }

    [Authorize]
    [HttpGet("[Action]/{guid}")]
    public async Task<IActionResult> GetShowByGuid(Guid guid)
    {
        var show = await _service.GetShowByGuidAsync(guid);

        if (show == null)
            return NotFound(Result<object>.Failure(null, "Nenhum show encontrado."));

        return Ok(Result<ShowDto>.Ok(show));
    }

    [Authorize]
    [HttpPut("[Action]")]
    public async Task<IActionResult> UpdateShow([FromBody] ShowUpdateDto showUpdateDto)
    {
        var updatedShow = await _service.UpdateShowAsync(showUpdateDto);

        if (updatedShow == null)
            return NotFound(Result<object>.Failure(null, "Show não encontrado para atualização."));

        return Ok(Result<ShowDto>.Ok(updatedShow, "Show atualizado com sucesso!"));
    }

    [Authorize]
    [HttpDelete("[Action]/{guid}")]
    public async Task<IActionResult> DeleteShow(Guid guid)
    {
        var deleted = await _service.DeleteShowAsync(guid);

        if (!deleted)
            return NotFound(Result<object>.Failure(null, "Show não encontrado para exclusão."));

        return Ok(Result<bool>.Ok(true, "Show removido com sucesso!"));
    }
}
