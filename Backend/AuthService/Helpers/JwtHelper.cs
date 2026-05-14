using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(Compte compte)
    {
        var key = _configuration["Jwt:Key"] ?? "SuperSecretKeyForLivraisonProject2026!";
        var issuer = _configuration["Jwt:Issuer"] ?? "Livraison.AuthService";
        var audience = _configuration["Jwt:Audience"] ?? "Livraison.ClientApps";

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, compte.Id.ToString()),
            new(ClaimTypes.Name, compte.Login),
            new(ClaimTypes.GivenName, compte.FullName),
            new(ClaimTypes.Role, compte.Role),
            new("Id", compte.Id.ToString()),
            new("Login", compte.Login),
            new("Role", compte.Role),
            new("FullName", compte.FullName)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
