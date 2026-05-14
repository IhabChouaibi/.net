using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivraisonFrontend.Helpers;
using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Services;

public abstract class ApiServiceBase
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly IHttpClientFactory _httpClientFactory;
    protected readonly ILogger Logger;

    protected ApiServiceBase(IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _httpClientFactory = httpClientFactory;
        Logger = logger;
    }

    protected HttpClient CreateClient() => _httpClientFactory.CreateClient("GatewayClient");

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        using var response = await CreateClient().GetAsync(endpoint);
        var content = await ReadAndEnsureSuccessAsync(response, endpoint);
        return DeserializeObject<T>(content);
    }

    protected async Task<T?> GetOptionalAsync<T>(string endpoint)
    {
        using var response = await CreateClient().GetAsync(endpoint);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Logger.LogInformation("Optional API resource not found on {Endpoint}.", endpoint);
            return default;
        }

        var content = await ReadAndEnsureSuccessAsync(response, endpoint);
        return DeserializeObject<T>(content);
    }

    protected async Task<PagedResult<T>> GetPagedAsync<T>(string endpoint, SearchFilterViewModel filters)
    {
        using var response = await CreateClient().GetAsync(endpoint);
        var content = await ReadAndEnsureSuccessAsync(response, endpoint);
        return DeserializePagedResult<T>(content, filters);
    }

    protected async Task PostAsync<TRequest>(string endpoint, TRequest payload)
    {
        using var response = await CreateClient().PostAsync(endpoint, BuildJsonContent(payload));
        await ReadAndEnsureSuccessAsync(response, endpoint);
    }

    protected async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
    {
        using var response = await CreateClient().PostAsync(endpoint, BuildJsonContent(payload));
        var content = await ReadAndEnsureSuccessAsync(response, endpoint);
        return DeserializeObject<TResponse>(content) ?? throw new ApplicationException("Réponse API vide.");
    }

    protected async Task PutAsync<TRequest>(string endpoint, TRequest payload)
    {
        using var response = await CreateClient().PutAsync(endpoint, BuildJsonContent(payload));
        await ReadAndEnsureSuccessAsync(response, endpoint);
    }

    protected async Task DeleteAsync(string endpoint)
    {
        using var response = await CreateClient().DeleteAsync(endpoint);
        await ReadAndEnsureSuccessAsync(response, endpoint);
    }

    protected static string BuildQueryString(SearchFilterViewModel filters)
    {
        var queryParts = new List<string>();

        AddQueryPart(queryParts, "search", filters.SearchTerm);
        AddQueryPart(queryParts, "status", filters.Status);
        AddQueryPart(queryParts, "sortBy", filters.SortBy);
        AddQueryPart(queryParts, "sortDirection", filters.SortDirection);
        AddQueryPart(queryParts, "page", filters.Page.ToString());
        AddQueryPart(queryParts, "pageSize", filters.PageSize.ToString());

        return queryParts.Count == 0 ? string.Empty : "?" + string.Join("&", queryParts);
    }

    private static void AddQueryPart(ICollection<string> queryParts, string key, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        queryParts.Add($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(value)}");
    }

    private static StringContent BuildJsonContent<TRequest>(TRequest payload)
    {
        var json = JsonSerializer.Serialize(payload, JsonOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private async Task<string> ReadAndEnsureSuccessAsync(HttpResponseMessage response, string endpoint)
    {
        var content = response.Content is null ? string.Empty : await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            Logger.LogWarning("Unauthorized API call on {Endpoint}.", endpoint);
            throw new UnauthorizedAccessException("Votre session a expiré. Veuillez vous reconnecter.");
        }

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            Logger.LogWarning("Forbidden API call on {Endpoint}.", endpoint);
            throw new ApplicationException("Accès refusé pour cette opération.");
        }

        if (response.IsSuccessStatusCode)
        {
            return content;
        }

        var errorMessage = ExtractErrorMessage(content);
        Logger.LogError("API call failed on {Endpoint} with status {StatusCode}. Message: {Message}", endpoint, response.StatusCode, errorMessage);
        throw new ApplicationException(errorMessage);
    }

    private static T? DeserializeObject<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    private static PagedResult<T> DeserializePagedResult<T>(string json, SearchFilterViewModel filters)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new PagedResult<T> { Page = filters.Page, PageSize = filters.PageSize };
        }

        var directPagedResult = JsonSerializer.Deserialize<PagedResult<T>>(json, JsonOptions);
        if (directPagedResult is not null && directPagedResult.Items.Count > 0)
        {
            return directPagedResult;
        }

        using var document = JsonDocument.Parse(json);

        if (document.RootElement.ValueKind == JsonValueKind.Array)
        {
            var arrayItems = JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
            return new PagedResult<T>
            {
                Items = arrayItems,
                Page = filters.Page,
                PageSize = filters.PageSize,
                TotalCount = arrayItems.Count
            };
        }

        var itemsElement = GetFirstProperty(document.RootElement, "items", "data", "results", "records");
        var items = itemsElement.HasValue
            ? JsonSerializer.Deserialize<List<T>>(itemsElement.Value.GetRawText(), JsonOptions) ?? new List<T>()
            : new List<T>();

        return new PagedResult<T>
        {
            Items = items,
            Page = GetInt(document.RootElement, filters.Page, "page", "currentPage"),
            PageSize = GetInt(document.RootElement, filters.PageSize, "pageSize", "size"),
            TotalCount = GetInt(document.RootElement, items.Count, "totalCount", "totalItems", "total")
        };
    }

    private static int GetInt(JsonElement root, int defaultValue, params string[] propertyNames)
    {
        var property = GetFirstProperty(root, propertyNames);
        if (!property.HasValue)
        {
            return defaultValue;
        }

        return property.Value.ValueKind switch
        {
            JsonValueKind.Number when property.Value.TryGetInt32(out var number) => number,
            JsonValueKind.String when int.TryParse(property.Value.GetString(), out var number) => number,
            _ => defaultValue
        };
    }

    private static JsonElement? GetFirstProperty(JsonElement root, params string[] propertyNames)
    {
        if (root.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        foreach (var property in root.EnumerateObject())
        {
            if (propertyNames.Any(name => string.Equals(name, property.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return property.Value;
            }
        }

        return null;
    }

    private static string ExtractErrorMessage(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return "Une erreur est survenue lors de la communication avec l'API.";
        }

        try
        {
            using var document = JsonDocument.Parse(json);
            foreach (var key in new[] { "message", "error", "title", "detail" })
            {
                var property = GetFirstProperty(document.RootElement, key);
                if (property.HasValue && property.Value.ValueKind == JsonValueKind.String)
                {
                    return property.Value.GetString() ?? "Une erreur est survenue lors de la communication avec l'API.";
                }
            }
        }
        catch (JsonException)
        {
            // Ignore parse failures and fallback to raw text.
        }

        return json;
    }
}
