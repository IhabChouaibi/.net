using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Helpers;

public class AuthSessionHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthSessionHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext?.Session
                                ?? throw new InvalidOperationException("Session indisponible.");

    public void SaveLoginSession(AuthResponseViewModel response)
    {
        response.Role = NormalizeRole(response.Role);
        JwtSessionHelper.StoreUserSession(Session, response);
    }

    public void ClearSession() => JwtSessionHelper.Clear(Session);

    public bool IsAuthenticated() => !string.IsNullOrWhiteSpace(GetToken());

    public bool IsAdmin() => string.Equals(GetRole(), "Admin", StringComparison.OrdinalIgnoreCase);

    public bool IsUser() => string.Equals(GetRole(), "User", StringComparison.OrdinalIgnoreCase);

    public string GetToken() => JwtSessionHelper.GetToken(Session);

    public string GetRole() => NormalizeRole(JwtSessionHelper.GetRole(Session));

    public string GetLogin() => JwtSessionHelper.GetLogin(Session);

    public int? GetUserId() => ReadNullableInt(JwtSessionHelper.UserIdKey);

    public int? GetCompteId() => ReadNullableInt(JwtSessionHelper.CompteIdKey);

    public int? GetClientId() => JwtSessionHelper.GetClientId(Session);

    public void SetClientId(int clientId)
    {
        if (clientId > 0)
        {
            Session.SetString(JwtSessionHelper.ClientIdKey, clientId.ToString());
        }
    }

    private int? ReadNullableInt(string key)
    {
        return int.TryParse(Session.GetString(key), out var value) && value > 0 ? value : null;
    }

    private static string NormalizeRole(string? role)
    {
        if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return "Admin";
        }

        if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
        {
            return "User";
        }

        return role?.Trim() ?? string.Empty;
    }
}
