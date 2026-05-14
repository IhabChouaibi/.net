using LivreurService.Helpers;
using LivreurService.Interfaces;
using LivreurService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LivreurService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LivreursController : ControllerBase
{
    private readonly ILivreurService _livreurService;

    public LivreursController(ILivreurService livreurService)
    {
        _livreurService = livreurService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] SearchRequest request) => Ok(await _livreurService.GetPagedAsync(request));

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _livreurService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var livreur = await _livreurService.GetByIdAsync(id);
        return livreur is null ? NotFound() : Ok(livreur);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Livreur livreur)
    {
        var created = await _livreurService.AddAsync(livreur);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Livreur livreur)
    {
        var updated = await _livreurService.UpdateAsync(id, livreur);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _livreurService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
