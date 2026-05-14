namespace AuthService.Models;

public class AuthResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CompteId { get; set; }

    public int? ClientId { get; set; }

    public string Login { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
}
