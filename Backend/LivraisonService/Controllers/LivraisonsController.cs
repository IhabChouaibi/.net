using LivraisonService.Helpers;
using LivraisonService.Interfaces;
using LivraisonService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LivraisonsController : ControllerBase
{
    private readonly ILivraisonService _livraisonService;

    public LivraisonsController(ILivraisonService livraisonService)
    {
        _livraisonService = livraisonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] SearchRequest request) => Ok(await _livraisonService.GetPagedAsync(request));

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _livraisonService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var adresse = await _livraisonService.GetByIdAsync(id);
        return adresse is null ? NotFound() : Ok(adresse);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(AdresseLivraison adresseLivraison)
    {
        var created = await _livraisonService.AddAsync(adresseLivraison);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, AdresseLivraison adresseLivraison)
    {
        var updated = await _livraisonService.UpdateAsync(id, adresseLivraison);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _livraisonService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("statuses")]
    public async Task<IActionResult> GetStatuses() => Ok(await _livraisonService.GetStatutsAsync());

    [HttpGet("statuses/{id:int}")]
    public async Task<IActionResult> GetStatusById(int id)
    {
        var statut = await _livraisonService.GetStatutByIdAsync(id);
        return statut is null ? NotFound() : Ok(statut);
    }

    [HttpPost("statuses")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateStatus(StatutLivraison statutLivraison)
    {
        var created = await _livraisonService.AddStatutAsync(statutLivraison);
        return CreatedAtAction(nameof(GetStatusById), new { id = created.Id }, created);
    }

    [HttpPut("statuses/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, StatutLivraison statutLivraison)
    {
        var updated = await _livraisonService.UpdateStatutAsync(id, statutLivraison);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("statuses/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStatus(int id)
    {
        var deleted = await _livraisonService.DeleteStatutAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
