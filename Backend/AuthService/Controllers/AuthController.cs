using AuthService.Helpers;
using AuthService.Interfaces;
using AuthService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts([FromQuery] SearchRequest request) =>
        Ok(await _authService.GetAccountsAsync(request));

    [Authorize(Roles = "Admin")]
    [HttpGet("accounts/{id:int}")]
    public async Task<IActionResult> GetAccount(int id)
    {
        var compte = await _authService.GetAccountByIdAsync(id);
        return compte is null ? NotFound() : Ok(compte);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("accounts/{id:int}")]
    public async Task<IActionResult> UpdateAccount(int id, Compte compte)
    {
        var updated = await _authService.UpdateAccountAsync(id, compte);
        return updated ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("accounts/{id:int}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var deleted = await _authService.DeleteAccountAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
