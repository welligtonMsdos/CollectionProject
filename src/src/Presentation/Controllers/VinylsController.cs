using Collection10Api.src.Application.Common;
using Collection10Api.src.Application.Dtos.Vinyl;
using Collection10Api.src.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collection10Api.src.Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
public class VinylsController : ApiControllerBase
{
    private readonly IVinylService _service;
    
    public VinylsController(IVinylService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] VinylCreateDto vinylCreateDto)
    {       
        var result = await _service.PostAsync(vinylCreateDto, UserId);

        return CreatedAtAction(nameof(GetByGuid), 
                               new { guid = result.Guid }, 
                               Result<VinylDto>.Ok(result, "Vinyl successfully created!"));
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var vinyls = await _service.GetAsync(UserId);

        return Ok(Result<IEnumerable<VinylDto>>.Ok(vinyls));
    }

    [HttpGet("ByCombo")]
    public async Task<IActionResult> GetByCombo()
    {
        var vinyls = await _service.GetByComboAsync(UserId);

        return Ok(Result<IEnumerable<VinylByComboDto>>.Ok(vinyls));
    }

    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> GetByGuid(Guid guid)
    {
        var vinyl = await _service.GetByGuidAsync(guid);

        if (vinyl is null)
            return NotFound(Result<object>.Failure("Vinyl not found."));

        return Ok(Result<VinylDto>.Ok(vinyl));
    }
    
    [HttpPut("{guid:guid}")]
    public async Task<IActionResult> Put(Guid guid, [FromBody] VinylUpdateDto vinylUpdateDto)
    {
        var updatedVinyl = await _service.PutAsync(guid, 
                                                   vinylUpdateDto, 
                                                   UserId);

        if (updatedVinyl is null)
            return NotFound(Result<object>.Failure("Vinyl not found for update."));

        return Ok(Result<VinylDto>.Ok(updatedVinyl, "Vinyl successfully updated!"));
    }
    
    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var deletedVinyl = await _service.DeleteAsync(guid);

        if (!deletedVinyl)
            return NotFound(Result<object>.Failure("Vinyl not found for deletion."));

        return Ok(Result<bool>.Ok(true, "Vinyl removed successfully!"));
    }
}
