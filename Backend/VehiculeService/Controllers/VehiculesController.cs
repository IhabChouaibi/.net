using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiculeService.Helpers;
using VehiculeService.Interfaces;
using VehiculeService.Models;

namespace VehiculeService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiculesController : ControllerBase
{
    private readonly IVehiculeService _vehiculeService;

    public VehiculesController(IVehiculeService vehiculeService)
    {
        _vehiculeService = vehiculeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] SearchRequest request) => Ok(await _vehiculeService.GetPagedAsync(request));

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _vehiculeService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vehicule = await _vehiculeService.GetByIdAsync(id);
        return vehicule is null ? NotFound() : Ok(vehicule);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(VehiculeRequest request)
    {
        var created = await _vehiculeService.AddAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, VehiculeRequest request)
    {
        var updated = await _vehiculeService.UpdateAsync(id, request);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _vehiculeService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
