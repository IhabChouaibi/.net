using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaiementService.Helpers;
using PaiementService.Interfaces;
using PaiementService.Models;

namespace PaiementService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaiementsController : ControllerBase
{
    private readonly IPaiementService _paiementService;

    public PaiementsController(IPaiementService paiementService)
    {
        _paiementService = paiementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] SearchRequest request) => Ok(await _paiementService.GetPagedAsync(request));

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _paiementService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var paiement = await _paiementService.GetByIdAsync(id);
        return paiement is null ? NotFound() : Ok(paiement);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Paiement paiement)
    {
        var created = await _paiementService.AddAsync(paiement);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Paiement paiement)
    {
        var updated = await _paiementService.UpdateAsync(id, paiement);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _paiementService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
