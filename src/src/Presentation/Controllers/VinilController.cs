using Collection10Api.src.Application.Common;
using Collection10Api.src.Application.Dtos.Vinil;
using Collection10Api.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collection10Api.src.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VinilController : Controller
{
    private readonly IVinilService _service;
    
    public VinilController(IVinilService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("[Action]")]
    public async Task<IActionResult> CreateVinil([FromBody] VinilCreateDto vinilCreateDto)
    {
        // O ValidationFilter já validou o DTO aqui
        var result = await _service.CreateVinilAsync(vinilCreateDto);

        return CreatedAtAction(nameof(GetVinilById), new { id = result.Id }, Result<VinilDto>.Ok(result, "Vinil criado com sucesso!"));
    }

    [Authorize]
    [HttpGet("[Action]")]
    public async Task<IActionResult> GetAllVinis()
    {
        var vinis = await _service.GetAllVinilsAsync();

        return Ok(Result<IEnumerable<VinilDto>>.Ok(vinis));
    }

    [Authorize]
    [HttpGet("[Action]/{id}")]
    public async Task<IActionResult> GetVinilById(int id)
    {
        var vinil = await _service.GetVinilByIdAsync(id);

        if (vinil == null)
            return NotFound(Result<object>.Failure(null, "Vinil não encontrado."));

        return Ok(Result<VinilDto>.Ok(vinil));
    }

    [Authorize]
    [HttpPut("[Action]")]
    public async Task<IActionResult> UpdateVinil([FromBody] VinilUpdateDto vinilUpdateDto)
    {
        var updatedVinil = await _service.UpdateVinilAsync(vinilUpdateDto);

        if (updatedVinil == null)
            return NotFound(Result<object>.Failure(null, "Vinil não encontrado para atualização."));

        return Ok(Result<VinilDto>.Ok(updatedVinil, "Vinil atualizado com sucesso!"));
    }

    [Authorize]
    [HttpDelete("[Action]/{id}")]
    public async Task<IActionResult> DeleteVinil(int id)
    {
        var deleted = await _service.DeleteVinilAsync(id);

        if (!deleted)
            return NotFound(Result<object>.Failure(null, "Vinil não encontrado para exclusão."));

        return Ok(Result<bool>.Ok(true, "Vinil removido com sucesso!"));
    }
}
