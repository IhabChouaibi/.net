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

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        using var response = await CreateClient().PostAsync("/auth/login", new StringContent(
            JsonSerializer.Serialize(request),
            System.Text.Encoding.UTF8,
            "application/json"));

        var content = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Identifiants invalides.");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(string.IsNullOrWhiteSpace(content) ? "Le login a échoué." : content);
        }

        return ParseAuthResponse(content);
    }

    public Task<AuthResponse> RegisterAsync(RegisterRequest request) => PostAsync<RegisterRequest, AuthResponse>("/auth/register", request);

    public Task<PagedResult<AccountModel>> GetAccountsAsync(SearchFilterViewModel filters)
        => GetPagedAsync<AccountModel>($"/auth/accounts{BuildQueryString(filters)}", filters);

    public Task<AccountModel?> GetAccountByIdAsync(int id) => GetAsync<AccountModel>($"/auth/accounts/{id}");

    public Task UpdateAccountAsync(int id, AccountModel model) => PutAsync($"/auth/accounts/{id}", model);

    public Task DeleteAccountAsync(int id) => DeleteAsync($"/auth/accounts/{id}");

    private static AuthResponse ParseAuthResponse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ApplicationException("La réponse du service d'authentification est vide.");
        }

        var response = JsonSerializer.Deserialize<AuthResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new AuthResponse();

        using var document = JsonDocument.Parse(json);
        response.Token = ReadString(document.RootElement, response.Token, "token", "jwt", "accessToken");
        response.Login = ReadString(document.RootElement, response.Login, "login", "username", "userName");
        response.Role = ReadString(document.RootElement, response.Role, "role", "userRole");
        response.FullName = ReadString(document.RootElement, response.FullName, "fullName", "name");
        response.Id = ReadInt(document.RootElement, response.Id, "id", "userId");

        if (string.IsNullOrWhiteSpace(response.Token))
        {
            throw new ApplicationException("Le token JWT n'a pas été retourné par l'API.");
        }

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

    private static int ReadInt(JsonElement root, int currentValue, params string[] propertyNames)
    {
        if (currentValue > 0)
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
}
