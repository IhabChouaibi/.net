using ClientService.Helpers;
using ClientService.Interfaces;
using ClientService.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] SearchRequest request) => Ok(await _clientService.GetPagedAsync(request));

    [HttpGet("all")]
    public async Task<IActionResult> GetAll() => Ok(await _clientService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _clientService.GetByIdAsync(id);
        return client is null ? NotFound() : Ok(client);
    }

    [HttpGet("by-compte/{compteId:int}")]
    public async Task<IActionResult> GetByCompteId(int compteId)
    {
        if (!CanAccessCompteId(compteId))
        {
            return Forbid();
        }

        var client = await _clientService.GetByCompteIdAsync(compteId);
        return client is null ? NotFound() : Ok(client);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(Client client)
    {
        try
        {
            var created = await _clientService.RegisterAsync(client);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ApplicationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Client client)
    {
        var created = await _clientService.AddAsync(client);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Client client)
    {
        var updated = await _clientService.UpdateAsync(id, client);
        return updated ? NoContent() : NotFound();
    }

    [HttpPut("by-compte/{compteId:int}")]
    public async Task<IActionResult> UpdateByCompteId(int compteId, Client client)
    {
        if (!CanAccessCompteId(compteId))
        {
            return Forbid();
        }

        var updated = await _clientService.UpdateByCompteIdAsync(compteId, client);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _clientService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private bool CanAccessCompteId(int compteId)
    {
        if (User.IsInRole("Admin"))
        {
            return true;
        }

        var rawId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("Id");
        return int.TryParse(rawId, out var currentCompteId) && currentCompteId == compteId;
    }
}
