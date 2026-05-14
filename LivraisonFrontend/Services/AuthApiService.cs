using System.Net;
using System.Text;
using System.Text.Json;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Services;

public class AuthApiService : ApiServiceBase, IAuthApiService
{
    public AuthApiService(IHttpClientFactory httpClientFactory, ILogger<AuthApiService> logger)
        : base(httpClientFactory, logger)
    {
    }

    public Task<AuthResponseViewModel> LoginAsync(LoginRequest request) => PostAuthAsync("/auth/login", request);

    public Task<AuthResponseViewModel> RegisterAsync(RegisterRequest request)
    {
        request.Role = "User";
        return PostAuthAsync("/auth/register", request);
    }

    public Task<PagedResult<AccountModel>> GetAccountsAsync(SearchFilterViewModel filters)
        => GetPagedAsync<AccountModel>($"/auth/accounts{BuildQueryString(filters)}", filters);

    public Task<AccountModel?> GetAccountByIdAsync(int id) => GetAsync<AccountModel>($"/auth/accounts/{id}");

    public Task UpdateAccountAsync(int id, AccountModel model) => PutAsync($"/auth/accounts/{id}", model);

    public Task DeleteAccountAsync(int id) => DeleteAsync($"/auth/accounts/{id}");

    private async Task<AuthResponseViewModel> PostAuthAsync<TRequest>(string endpoint, TRequest request)
    {
        try
        {
            using var response = await CreateClient().PostAsync(endpoint, new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Identifiants invalides.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(ExtractAuthErrorMessage(content, response.StatusCode));
            }

            var parsed = ParseAuthResponse(content);
            if (!parsed.Success && !string.IsNullOrWhiteSpace(parsed.Message))
            {
                throw new ApplicationException(parsed.Message);
            }

            return parsed;
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (TaskCanceledException exception)
        {
            Logger.LogError(exception, "Timeout on {Endpoint}.", endpoint);
            throw new ApplicationException("Service indisponible. Le délai de réponse est dépassé.");
        }
        catch (HttpRequestException exception)
        {
            Logger.LogError(exception, "Gateway unavailable on {Endpoint}.", endpoint);
            throw new ApplicationException("Service indisponible. L'API Gateway est inaccessible.");
        }
        catch (JsonException exception)
        {
            Logger.LogError(exception, "Invalid auth payload on {Endpoint}.", endpoint);
            throw new ApplicationException("La réponse du service d'authentification est invalide.");
        }
    }

    private static AuthResponseViewModel ParseAuthResponse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ApplicationException("La réponse du service d'authentification est vide.");
        }

        var response = JsonSerializer.Deserialize<AuthResponseViewModel>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new AuthResponseViewModel();

        using var document = JsonDocument.Parse(json);
        response.Token = ReadString(document.RootElement, response.Token, "token", "jwt", "accessToken");
        response.Login = ReadString(document.RootElement, response.Login, "login", "username", "userName");
        response.Role = NormalizeRole(ReadString(document.RootElement, response.Role, "role", "userRole"));
        response.FullName = ReadString(document.RootElement, response.FullName, "fullName", "name");
        response.UserId = ReadNullableInt(document.RootElement, response.UserId, "userId", "id");
        response.CompteId = ReadNullableInt(document.RootElement, response.CompteId, "compteId", "id", "userId");
        response.ClientId = ReadNullableInt(document.RootElement, response.ClientId, "clientId");
        response.Id = ReadNullableInt(document.RootElement, response.Id, "id");

        return response;
    }

    private static string ReadString(JsonElement root, string currentValue, params string[] propertyNames)
    {
        if (!string.IsNullOrWhiteSpace(currentValue))
        {
            return currentValue;
        }

        foreach (var property in root.EnumerateObject())
        {
            if (propertyNames.Any(name => string.Equals(name, property.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return property.Value.ToString();
            }
        }

        return currentValue;
    }

    private static int? ReadNullableInt(JsonElement root, int? currentValue, params string[] propertyNames)
    {
        if (currentValue.GetValueOrDefault() > 0)
        {
            return currentValue;
        }

        foreach (var property in root.EnumerateObject())
        {
            if (!propertyNames.Any(name => string.Equals(name, property.Name, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            if (property.Value.ValueKind == JsonValueKind.Number && property.Value.TryGetInt32(out var numberValue))
            {
                return numberValue;
            }

            if (property.Value.ValueKind == JsonValueKind.String && int.TryParse(property.Value.GetString(), out numberValue))
            {
                return numberValue;
            }
        }

        return currentValue;
    }

    private static string NormalizeRole(string role)
    {
        if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return "Admin";
        }

        if (string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
        {
            return "User";
        }

        return role;
    }

    private static string ExtractAuthErrorMessage(string content, HttpStatusCode statusCode)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return statusCode switch
            {
                HttpStatusCode.BadRequest => "La requête d'authentification est invalide.",
                HttpStatusCode.Forbidden => "Accès refusé.",
                HttpStatusCode.NotFound => "Service d'authentification introuvable.",
                HttpStatusCode.InternalServerError => "Le service d'authentification a rencontré une erreur.",
                _ => "Le service d'authentification a échoué."
            };
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            foreach (var key in new[] { "message", "error", "title", "detail" })
            {
                if (document.RootElement.TryGetProperty(key, out var property) && property.ValueKind == JsonValueKind.String)
                {
                    return property.GetString() ?? "Le service d'authentification a échoué.";
                }
            }
        }
        catch (JsonException)
        {
        }

        return content;
    }
}
