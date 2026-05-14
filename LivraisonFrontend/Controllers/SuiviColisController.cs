using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public class SuiviColisController : AppController
{
    private readonly IColisApiService _colisApiService;
    private readonly ILivraisonApiService _livraisonApiService;
    private readonly IProfilApiService _profilApiService;
    private readonly ILogger<SuiviColisController> _logger;

    public SuiviColisController(
        IColisApiService colisApiService,
        ILivraisonApiService livraisonApiService,
        IProfilApiService profilApiService,
        SessionManager sessionManager,
        ILogger<SuiviColisController> logger)
        : base(sessionManager)
    {
        _colisApiService = colisApiService;
        _livraisonApiService = livraisonApiService;
        _profilApiService = profilApiService;
        _logger = logger;
    }

    public Task<IActionResult> Index(SuiviColisFilterViewModel filters) => ExecuteAsync(async () =>
    {
        var token = HttpContext.Session.GetString("Token");
        var role = HttpContext.Session.GetString("Role");
        Console.WriteLine($"TOKEN={(string.IsNullOrWhiteSpace(token) ? "(null)" : "present")}");
        Console.WriteLine($"ROLE={role ?? "(null)"}");
        _logger.LogInformation(
            "SuiviColis/Index. Token present: {HasToken}. Role: {Role}. Controller: SuiviColis. Action: Index.",
            !string.IsNullOrWhiteSpace(token),
            role ?? "(null)");

        var accessRedirect = RequireUserAccess();
        if (accessRedirect is not null)
        {
            if (accessRedirect is RedirectToActionResult redirect)
            {
                Console.WriteLine($"REDIRECT TO={redirect.ControllerName}/{redirect.ActionName}");
            }
            return accessRedirect;
        }

        Console.WriteLine("REDIRECT TO=(none)");
        ViewData["Title"] = "Suivi de colis";
        return View(await BuildPageViewModelAsync(filters, "Suivi de colis", "Consultez l'état de vos livraisons"));
    }, fallbackAction: "Login", fallbackController: "Auth");

    public Task<IActionResult> MesColis(SuiviColisFilterViewModel filters) => ExecuteAsync(async () =>
    {
        var token = HttpContext.Session.GetString("Token");
        var role = HttpContext.Session.GetString("Role");
        Console.WriteLine($"TOKEN={(string.IsNullOrWhiteSpace(token) ? "(null)" : "present")}");
        Console.WriteLine($"ROLE={role ?? "(null)"}");
        _logger.LogInformation(
            "SuiviColis/MesColis. Token present: {HasToken}. Role: {Role}. Controller: SuiviColis. Action: MesColis.",
            !string.IsNullOrWhiteSpace(token),
            role ?? "(null)");

        var accessRedirect = RequireUserAccess();
        if (accessRedirect is not null)
        {
            if (accessRedirect is RedirectToActionResult redirect)
            {
                Console.WriteLine($"REDIRECT TO={redirect.ControllerName}/{redirect.ActionName}");
            }
            return accessRedirect;
        }

        Console.WriteLine("REDIRECT TO=(none)");
        ViewData["Title"] = "Mes colis";
        return View(await BuildPageViewModelAsync(filters, "Mes colis", "Retrouvez l'ensemble de vos colis et leur statut"));
    }, fallbackAction: "Login", fallbackController: "Auth");

    public Task<IActionResult> Details(int id) => ExecuteAsync(async () =>
    {
        var token = HttpContext.Session.GetString("Token");
        var role = HttpContext.Session.GetString("Role");
        Console.WriteLine($"TOKEN={(string.IsNullOrWhiteSpace(token) ? "(null)" : "present")}");
        Console.WriteLine($"ROLE={role ?? "(null)"}");
        _logger.LogInformation(
            "SuiviColis/Details. Token present: {HasToken}. Role: {Role}. Controller: SuiviColis. Action: Details.",
            !string.IsNullOrWhiteSpace(token),
            role ?? "(null)");

        var accessRedirect = RequireUserAccess();
        if (accessRedirect is not null)
        {
            if (accessRedirect is RedirectToActionResult redirect)
            {
                Console.WriteLine($"REDIRECT TO={redirect.ControllerName}/{redirect.ActionName}");
            }
            return accessRedirect;
        }

        Console.WriteLine("REDIRECT TO=(none)");
        var profile = await _profilApiService.GetProfileAsync(SessionManager.UserId);
        if (profile is null || profile.Id <= 0)
        {
            ShowError("Aucun profil client n'est lié à ce compte.");
            return RedirectToAction(nameof(Index));
        }

        var colisItems = await LoadClientColisAsync(profile.Id);
        var item = colisItems.FirstOrDefault(x => x.Id == id);
        if (item is null)
        {
            return RedirectToAction("NotFoundPage", "Error");
        }

        ViewData["Title"] = $"Colis #{item.Id}";
        return View(item);
    }, fallbackAction: nameof(Index));

    private async Task<SuiviColisPageViewModel> BuildPageViewModelAsync(
        SuiviColisFilterViewModel filters,
        string title,
        string subtitle)
    {
        var profile = await _profilApiService.GetProfileAsync(SessionManager.UserId);
        if (profile is null || profile.Id <= 0)
        {
            ShowError("Aucun profil client n'est lié à ce compte.");
            return new SuiviColisPageViewModel
            {
                Title = title,
                Subtitle = subtitle,
                Filters = filters
            };
        }

        var items = await LoadClientColisAsync(profile.Id);
        var filteredItems = items
            .Where(item => string.IsNullOrWhiteSpace(filters.SearchTerm)
                           || item.Id.ToString().Contains(filters.SearchTerm, StringComparison.OrdinalIgnoreCase)
                           || item.Libelle.Contains(filters.SearchTerm, StringComparison.OrdinalIgnoreCase)
                           || item.Statut.Contains(filters.SearchTerm, StringComparison.OrdinalIgnoreCase))
            .Where(item => string.IsNullOrWhiteSpace(filters.Status)
                           || string.Equals(item.Statut, filters.Status, StringComparison.OrdinalIgnoreCase))
            .Where(item => string.IsNullOrWhiteSpace(filters.Ville)
                           || item.Ville.Contains(filters.Ville, StringComparison.OrdinalIgnoreCase))
            .Where(item => !filters.DateLivraison.HasValue || item.DateLivraison.Date == filters.DateLivraison.Value.Date)
            .OrderByDescending(item => item.DateLivraison)
            .ToList();

        return new SuiviColisPageViewModel
        {
            Title = title,
            Subtitle = subtitle,
            Filters = filters,
            Items = filteredItems
        };
    }

    private async Task<List<SuiviColisItemViewModel>> LoadClientColisAsync(int clientId)
    {
        var colis = await _colisApiService.GetByClientIdAsync(clientId);
        var livraisons = await _livraisonApiService.GetAllAsync();

        return colis.Select(item =>
        {
            var livraison = livraisons.FirstOrDefault(x => x.ColisId == item.Id);
            return new SuiviColisItemViewModel
            {
                Id = item.Id,
                Libelle = item.Libelle,
                DateLivraison = item.DateLivraison,
                Montant = item.Montant,
                Poids = item.Poids,
                Volume = item.Volume,
                ClientId = item.ClientId,
                LivreurId = item.LivreurId,
                StatutLivraisonId = item.StatutLivraisonId,
                Statut = item.StatutLivraisonLabel,
                Adresse = livraison?.Adresse ?? "Adresse indisponible",
                Ville = livraison?.Ville ?? "Ville indisponible",
                CodePostal = livraison?.CodePostal ?? string.Empty
            };
        }).ToList();
    }
}
