using System.Net.Http.Headers;
using System.Text.Json;
using DashboardService.Helpers;
using DashboardService.Interfaces;
using DashboardService.Models;
using Microsoft.Extensions.Options;

namespace DashboardService.Services;

public class DashboardAggregatorService : IDashboardService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ServiceUrls _serviceUrls;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public DashboardAggregatorService(
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        IOptions<ServiceUrls> serviceUrls)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _serviceUrls = serviceUrls.Value;
    }

    public async Task<DashboardStats> GetDashboardAsync()
    {
        var clients = await GetAsync<List<ClientItem>>($"{_serviceUrls.ClientService}/api/clients/all") ?? new List<ClientItem>();
        var livreurs = await GetAsync<List<LivreurItem>>($"{_serviceUrls.LivreurService}/api/livreurs/all") ?? new List<LivreurItem>();
        var colis = await GetAsync<List<ColisItem>>($"{_serviceUrls.ColisService}/api/colis/all") ?? new List<ColisItem>();
        var paiements = await GetAsync<List<PaiementItem>>($"{_serviceUrls.PaiementService}/api/paiements/all") ?? new List<PaiementItem>();

        var livreurMap = livreurs.ToDictionary(x => x.Id, x => x.RaisonSocial);

        return new DashboardStats
        {
            TotalClients = clients.Count,
            TotalLivreurs = livreurs.Count,
            TotalColis = colis.Count,
            TotalRevenue = paiements.Sum(x => x.Montant),
            DeliveredPackages = colis.Count(x => x.StatutLivraisonId == 3),
            PendingPackages = colis.Count(x => x.StatutLivraisonId == 1),
            PackageStatusChart = colis
                .GroupBy(x => x.StatutLivraisonLabel)
                .Select(g => new ChartPoint { Label = g.Key, Value = g.Count() })
                .OrderByDescending(x => x.Value)
                .ToList(),
            PaymentByMonthChart = paiements
                .GroupBy(x => x.DatePaiement.ToString("yyyy-MM"))
                .Select(g => new ChartPoint { Label = g.Key, Value = g.Sum(x => x.Montant) })
                .OrderBy(x => x.Label)
                .ToList(),
            LatestDeliveries = colis
                .OrderByDescending(x => x.DateLivraison)
                .Take(5)
                .Select(x => new RecentDelivery
                {
                    TrackingNumber = $"COL-{x.Id:0000}",
                    DriverName = livreurMap.TryGetValue(x.LivreurId, out var driverName) ? driverName : $"Livreur #{x.LivreurId}",
                    Status = x.StatutLivraisonLabel,
                    ScheduledDate = x.DateLivraison
                })
                .ToList(),
            LatestClients = clients
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => new RecentClient
                {
                    FullName = $"{x.Prenom} {x.Nom}",
                    Email = $"{x.Prenom.ToLowerInvariant()}.{x.Nom.ToLowerInvariant().Replace(" ", string.Empty)}@demo.local",
                    CreatedAt = DateTime.UtcNow.AddDays(-x.Id)
                })
                .ToList(),
            RecentActivities = BuildActivities(clients, colis, paiements)
        };
    }

    private async Task<T?> GetAsync<T>(string url)
    {
        var client = _httpClientFactory.CreateClient();
        var token = TokenForwardingHelper.ExtractBearerToken(_httpContextAccessor.HttpContext?.Request ?? new DefaultHttpContext().Request);

        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
    }

    private static IList<ActivityItem> BuildActivities(
        IEnumerable<ClientItem> clients,
        IEnumerable<ColisItem> colis,
        IEnumerable<PaiementItem> paiements)
    {
        var activities = new List<ActivityItem>();

        activities.AddRange(clients
            .OrderByDescending(x => x.Id)
            .Take(3)
            .Select(x => new ActivityItem
            {
                Title = "Nouveau client",
                Description = $"{x.Prenom} {x.Nom} - {x.Ville}",
                Date = DateTime.UtcNow.AddHours(-x.Id),
                Status = "Info"
            }));

        activities.AddRange(colis
            .OrderByDescending(x => x.DateLivraison)
            .Take(3)
            .Select(x => new ActivityItem
            {
                Title = "Mise a jour colis",
                Description = $"{x.Libelle} - {x.StatutLivraisonLabel}",
                Date = x.DateLivraison,
                Status = x.StatutLivraisonLabel
            }));

        activities.AddRange(paiements
            .OrderByDescending(x => x.DatePaiement)
            .Take(3)
            .Select(x => new ActivityItem
            {
                Title = "Paiement enregistre",
                Description = $"{x.Montant:N2} DT - {x.ModePaiement}",
                Date = x.DatePaiement,
                Status = "Succes"
            }));

        return activities
            .OrderByDescending(x => x.Date)
            .Take(8)
            .ToList();
    }
}
