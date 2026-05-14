using ColisService.Helpers;
using ColisService.Interfaces;
using ColisService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColisService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ColisController : ControllerBase
{
    private readonly IColisService _colisService;

    public ColisController(IColisService colisService)
    {
        _colisService = colisService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] SearchRequest request) => Ok(await _colisService.GetPagedAsync(request));

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _colisService.GetAllAsync());

    [HttpGet("client/{clientId:int}")]
    public async Task<IActionResult> GetByClientId(int clientId) => Ok(await _colisService.GetByClientIdAsync(clientId));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var colis = await _colisService.GetByIdAsync(id);
        return colis is null ? NotFound() : Ok(colis);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Colis colis)
    {
        var created = await _colisService.AddAsync(colis);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Colis colis)
    {
        var updated = await _colisService.UpdateAsync(id, colis);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _colisService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
