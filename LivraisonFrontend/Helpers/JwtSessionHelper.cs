using System.Security.Claims;
using System.Text;
using System.Text.Json;
using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Helpers;

public static class JwtSessionHelper
{
    public const string TokenKey = "Token";
    public const string LoginKey = "Login";
    public const string RoleKey = "Role";
    public const string FullNameKey = "FullName";
    public const string UserIdKey = "UserId";
    public const string CompteIdKey = "CompteId";
    public const string ClientIdKey = "ClientId";
    private const string LegacyTokenKey = "AuthToken";
    private const string LegacyLoginKey = "UserLogin";
    private const string LegacyRoleKey = "UserRole";
    private const string LegacyFullNameKey = "UserFullName";

    public static void StoreUserSession(ISession session, AuthResponseViewModel response)
    {
        session.SetString(TokenKey, response.Token);
        session.SetString(LoginKey, response.Login);
        session.SetString(RoleKey, response.Role);
        session.SetString(FullNameKey, response.FullName);
        session.SetString(UserIdKey, response.EffectiveUserId > 0 ? response.EffectiveUserId.ToString() : string.Empty);
        session.SetString(CompteIdKey, response.CompteId.GetValueOrDefault() > 0
            ? response.CompteId!.Value.ToString()
            : response.EffectiveUserId > 0 ? response.EffectiveUserId.ToString() : string.Empty);
        session.SetString(ClientIdKey, response.ClientId.GetValueOrDefault() > 0 ? response.ClientId!.Value.ToString() : string.Empty);
        session.SetString(LegacyTokenKey, response.Token);
        session.SetString(LegacyLoginKey, response.Login);
        session.SetString(LegacyRoleKey, response.Role);
        session.SetString(LegacyFullNameKey, response.FullName);
    }

    public static void Clear(ISession session)
    {
        session.Remove(TokenKey);
        session.Remove(LoginKey);
        session.Remove(RoleKey);
        session.Remove(FullNameKey);
        session.Remove(UserIdKey);
        session.Remove(CompteIdKey);
        session.Remove(ClientIdKey);
        session.Remove(LegacyTokenKey);
        session.Remove(LegacyLoginKey);
        session.Remove(LegacyRoleKey);
        session.Remove(LegacyFullNameKey);
    }

    public static string GetToken(ISession session) => GetValue(session, TokenKey, LegacyTokenKey);

    public static string GetLogin(ISession session) => GetValue(session, LoginKey, LegacyLoginKey);

    public static string GetRole(ISession session) => GetValue(session, RoleKey, LegacyRoleKey);

    public static string GetFullName(ISession session) => GetValue(session, FullNameKey, LegacyFullNameKey);

    public static int GetUserId(ISession session)
        => int.TryParse(session.GetString(UserIdKey), out var userId) ? userId : 0;

    public static int GetCompteId(ISession session)
        => int.TryParse(session.GetString(CompteIdKey), out var compteId) ? compteId : 0;

    public static int? GetClientId(ISession session)
        => int.TryParse(session.GetString(ClientIdKey), out var clientId) ? clientId : null;

    public static bool HasToken(ISession session) => !string.IsNullOrWhiteSpace(GetToken(session));

    public static IEnumerable<Claim> BuildClaims(AuthResponseViewModel response)
    {
        var claims = ParseClaimsFromJwt(response.Token).ToList();

        AddClaimIfMissing(claims, ClaimTypes.Name, response.Login);
        AddClaimIfMissing(claims, ClaimTypes.GivenName, response.FullName);
        AddClaimIfMissing(claims, ClaimTypes.Role, response.Role);

        return claims;
    }

    private static void AddClaimIfMissing(ICollection<Claim> claims, string claimType, string value)
    {
        if (string.IsNullOrWhiteSpace(value) || claims.Any(c => c.Type == claimType))
        {
            return;
        }

        claims.Add(new Claim(claimType, value));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt) || jwt.Split('.').Length < 2)
        {
            return Array.Empty<Claim>();
        }

        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var claims = new List<Claim>();

        using var document = JsonDocument.Parse(jsonBytes);
        foreach (var property in document.RootElement.EnumerateObject())
        {
            if (property.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in property.Value.EnumerateArray())
                {
                    claims.Add(new Claim(MapClaimType(property.Name), item.ToString()));
                }

                continue;
            }

            claims.Add(new Claim(MapClaimType(property.Name), property.Value.ToString()));
        }

        return claims;
    }

    private static string MapClaimType(string claimType) => claimType.ToLowerInvariant() switch
    {
        "role" or "roles" or "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" => ClaimTypes.Role,
        "unique_name" or "preferred_username" or "name" or "sub" => ClaimTypes.Name,
        "given_name" => ClaimTypes.GivenName,
        "email" => ClaimTypes.Email,
        _ => claimType
    };

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        base64 = base64.Replace('-', '+').Replace('_', '/');
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }

    private static string GetValue(ISession session, string primaryKey, string legacyKey)
        => session.GetString(primaryKey)
           ?? session.GetString(legacyKey)
           ?? string.Empty;
}
