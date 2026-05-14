using System.Text.Json;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Services;

public class DashboardApiService : ApiServiceBase, IDashboardApiService
{
    public DashboardApiService(IHttpClientFactory httpClientFactory, ILogger<DashboardApiService> logger)
        : base(httpClientFactory, logger)
    {
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var viewModel = await GetAsync<DashboardViewModel>("/dashboard");
        if (viewModel is not null)
        {
            return EnsureCollections(viewModel);
        }

        using var response = await CreateClient().GetAsync("/dashboard");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(string.IsNullOrWhiteSpace(content) ? "Impossible de charger le dashboard." : content);
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return new DashboardViewModel();
        }

        using var document = JsonDocument.Parse(content);
        var dashboard = new DashboardViewModel
        {
            TotalClients = ReadInt(document.RootElement, "totalClients", "clientsCount"),
            TotalLivreurs = ReadInt(document.RootElement, "totalLivreurs", "driversCount"),
            TotalColis = ReadInt(document.RootElement, "totalColis", "packagesCount"),
            TotalRevenue = ReadDecimal(document.RootElement, "totalRevenue", "montantTotal"),
            DeliveredPackages = ReadInt(document.RootElement, "deliveredPackages", "colisLivres"),
            PendingPackages = ReadInt(document.RootElement, "pendingPackages", "colisEnAttente")
        };

        return EnsureCollections(dashboard);
    }

    private static DashboardViewModel EnsureCollections(DashboardViewModel model)
    {
        model.PackageStatusChart ??= new List<ChartPointViewModel>();
        model.PaymentByMonthChart ??= new List<ChartPointViewModel>();
        model.RecentActivities ??= new List<ActivityItemViewModel>();
        model.LatestDeliveries ??= new List<RecentDeliveryViewModel>();
        model.LatestClients ??= new List<RecentClientViewModel>();
        return model;
    }

    private static int ReadInt(JsonElement root, params string[] propertyNames)
    {
        foreach (var property in root.EnumerateObject())
        {
            if (!propertyNames.Any(name => string.Equals(name, property.Name, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            if (property.Value.ValueKind == JsonValueKind.Number && property.Value.TryGetInt32(out var result))
            {
                return result;
            }

            if (property.Value.ValueKind == JsonValueKind.String && int.TryParse(property.Value.GetString(), out result))
            {
                return result;
            }
        }

        return 0;
    }

    private static decimal ReadDecimal(JsonElement root, params string[] propertyNames)
    {
        foreach (var property in root.EnumerateObject())
        {
            if (!propertyNames.Any(name => string.Equals(name, property.Name, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            if (property.Value.ValueKind == JsonValueKind.Number && property.Value.TryGetDecimal(out var result))
            {
                return result;
            }

            if (property.Value.ValueKind == JsonValueKind.String && decimal.TryParse(property.Value.GetString(), out result))
            {
                return result;
            }
        }

        return 0;
    }
}
