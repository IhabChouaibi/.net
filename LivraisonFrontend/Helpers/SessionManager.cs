using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Helpers;

public class SessionManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext?.Session
                                ?? throw new InvalidOperationException("Session indisponible.");

    public string Token => JwtSessionHelper.GetToken(Session);
    public int UserId => JwtSessionHelper.GetUserId(Session);
    public int CompteId => JwtSessionHelper.GetCompteId(Session);
    public int? ClientId => JwtSessionHelper.GetClientId(Session);
    public string Login => JwtSessionHelper.GetLogin(Session);
    public string Role => JwtSessionHelper.GetRole(Session);
    public string FullName => JwtSessionHelper.GetFullName(Session);
    public bool HasActiveSession => JwtSessionHelper.HasToken(Session);
    public bool IsAdmin => string.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase);
    public bool IsUser => string.Equals(Role, "User", StringComparison.OrdinalIgnoreCase);

    public string GetToken() => Token;
    public string GetRole() => Role;
    public string GetLogin() => Login;
    public int GetUserId() => UserId;
    public int GetCompteId() => CompteId;
    public int? GetClientId() => ClientId;
    public bool IsAuthenticated() => HasActiveSession;
    public bool IsAuthenticatedUser() => HasActiveSession;
    public bool IsAdminUser() => IsAdmin;
    public bool IsUserClient() => IsUser;

    public void Store(AuthResponseViewModel response) => JwtSessionHelper.StoreUserSession(Session, response);

    public void SetClientId(int? clientId)
    {
        if (clientId.GetValueOrDefault() > 0)
        {
            Session.SetString(JwtSessionHelper.ClientIdKey, clientId!.Value.ToString());
            return;
        }

        Session.Remove(JwtSessionHelper.ClientIdKey);
    }

    public void Clear() => JwtSessionHelper.Clear(Session);
}
