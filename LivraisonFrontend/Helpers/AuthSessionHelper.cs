namespace LivraisonFrontend.Helpers;

public class AuthSessionHelper
{
    private readonly SessionManager _sessionManager;

    public AuthSessionHelper(SessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }

    public string GetToken() => _sessionManager.Token;
    public string GetRole() => _sessionManager.Role;
    public string GetLogin() => _sessionManager.Login;
    public int GetUserId() => _sessionManager.UserId;
    public bool IsAuthenticated() => _sessionManager.IsAuthenticated;
    public bool IsAdmin() => _sessionManager.IsAdmin;
    public bool IsUser() => _sessionManager.IsUser;
}
