namespace LivraisonFrontend.Models;

public class AuthResponse
{
    public int Id { get; set; }

    public int UserId => Id;

    public int ClientId { get; set; }

    public string Token { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}
