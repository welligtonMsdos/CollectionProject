using Collection10Api.src.Application.Common;
using Collection10Api.src.Application.Dtos.Vinyl;
using Collection10Api.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collection10Api.src.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VinylController : Controller
{
    private readonly IVinylService _service;
    
    public VinylController(IVinylService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("[Action]")]
    public async Task<IActionResult> CreateVinyl([FromBody] VinylCreateDto vinylCreateDto)
    {       
        var result = await _service.CreateVinylAsync(vinylCreateDto);

        return CreatedAtAction(nameof(GetVinylById), new { id = result.Id }, Result<VinylDto>.Ok(result, "Vinyl successfully created!"));
    }

    [Authorize]
    [HttpGet("[Action]")]
    public async Task<IActionResult> GetAllVinyls()
    {
        var vinyls = await _service.GetAllVinylsAsync();

        return Ok(Result<IEnumerable<VinylDto>>.Ok(vinyls));
    }

    [Authorize]
    [HttpGet("[Action]/{id}")]
    public async Task<IActionResult> GetVinylById(int id)
    {
        var vinyl = await _service.GetVinylByIdAsync(id);

        if (vinyl == null)
            return NotFound(Result<object>.Failure("Vinyl not found."));

        return Ok(Result<VinylDto>.Ok(vinyl));
    }

    [Authorize]
    [HttpPut("[Action]")]
    public async Task<IActionResult> UpdateVinyl([FromBody] VinylUpdateDto vinylUpdateDto)
    {
        var updatedVinyl = await _service.UpdateVinylAsync(vinylUpdateDto);

        if (updatedVinyl == null)
            return NotFound(Result<object>.Failure("Vinyl not found for update."));

        return Ok(Result<VinylDto>.Ok(updatedVinyl, "Vinyl successfully updated!"));
    }

    [Authorize]
    [HttpDelete("[Action]/{id}")]
    public async Task<IActionResult> DeleteVinyl(int id)
    {
        var deletedVinyl = await _service.DeleteVinylAsync(id);

        if (!deletedVinyl)
            return NotFound(Result<object>.Failure("Vinyl not found for deletion."));

        return Ok(Result<bool>.Ok(true, "Vinyl removed successfully!"));
    }
}
