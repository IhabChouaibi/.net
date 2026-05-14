using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public class SuiviColisController : Controller
{
    private readonly ClientAccessGuard _clientAccessGuard;
    private readonly AuthSessionHelper _authSessionHelper;
    private readonly ISuiviColisApiService _suiviColisApiService;
    private readonly IProfilClientApiService _profilClientApiService;

    public SuiviColisController(
        ClientAccessGuard clientAccessGuard,
        AuthSessionHelper authSessionHelper,
        ISuiviColisApiService suiviColisApiService,
        IProfilClientApiService profilClientApiService)
    {
        _clientAccessGuard = clientAccessGuard;
        _authSessionHelper = authSessionHelper;
        _suiviColisApiService = suiviColisApiService;
        _profilClientApiService = profilClientApiService;
    }

    public Task<IActionResult> Index(SuiviColisFilterViewModel filters) =>
        RenderListAsync(filters, "Suivi Colis", "Consultez la timeline et l'état de vos colis.", nameof(Index));

    public Task<IActionResult> MesColis(SuiviColisFilterViewModel filters) =>
        RenderListAsync(filters, "Mes Colis", "Retrouvez l'ensemble de vos colis dans un espace unique.", nameof(MesColis));

    public async Task<IActionResult> Details(int id)
    {
        var accessRedirect = _clientAccessGuard.Guard(this);
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        try
        {
            var clientId = await _profilClientApiService.ResolveClientIdAsync();
            if (clientId.GetValueOrDefault() <= 0)
            {
                TempData["Notification.Type"] = "info";
                TempData["Notification.Title"] = "Information";
                TempData["Notification.Message"] = "Profil introuvable.";
                return RedirectToAction(nameof(Index));
            }

            var item = await _suiviColisApiService.GetClientColisDetailsAsync(id);
            if (item is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            ViewData["Title"] = $"Colis #{item.Id}";
            return View(item);
        }
        catch (UnauthorizedAccessException)
        {
            _authSessionHelper.ClearSession();
            return RedirectToAction("Login", "Auth");
        }
        catch (ApplicationException exception)
        {
            TempData["Notification.Type"] = "error";
            TempData["Notification.Title"] = "Erreur";
            TempData["Notification.Message"] = exception.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    private async Task<IActionResult> RenderListAsync(
        SuiviColisFilterViewModel filters,
        string title,
        string subtitle,
        string viewName)
    {
        var accessRedirect = _clientAccessGuard.Guard(this);
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        try
        {
            ViewData["Title"] = title;
            var clientId = await _profilClientApiService.ResolveClientIdAsync();
            var items = await _suiviColisApiService.GetClientColisAsync();
            var filteredItems = ApplyFilters(items, filters);

            var model = new SuiviColisPageViewModel
            {
                Title = title,
                Subtitle = subtitle,
                Filters = filters,
                Items = filteredItems,
                EmptyMessage = "Aucun colis disponible pour le moment",
                EmptyDescription = clientId.GetValueOrDefault() <= 0
                    ? "Votre compte est connecté, mais aucun profil client exploitable n'a encore été retrouvé."
                    : filteredItems.Count == 0 && items.Count > 0
                        ? "Aucun colis ne correspond à votre recherche actuelle."
                        : "Vos futurs colis apparaîtront ici automatiquement."
            };

            return View(viewName, model);
        }
        catch (UnauthorizedAccessException)
        {
            _authSessionHelper.ClearSession();
            return RedirectToAction("Login", "Auth");
        }
        catch (ApplicationException exception)
        {
            TempData["Notification.Type"] = "error";
            TempData["Notification.Title"] = "Erreur";
            TempData["Notification.Message"] = exception.Message;
            return View(viewName, new SuiviColisPageViewModel
            {
                Title = title,
                Subtitle = subtitle,
                Filters = filters
            });
        }
    }

    private static IReadOnlyList<SuiviColisItemViewModel> ApplyFilters(
        IReadOnlyList<SuiviColisItemViewModel> items,
        SuiviColisFilterViewModel filters)
    {
        return items
            .Where(item => string.IsNullOrWhiteSpace(filters.SearchTerm)
                           || item.Id.ToString().Contains(filters.SearchTerm, StringComparison.OrdinalIgnoreCase)
                           || item.Libelle.Contains(filters.SearchTerm, StringComparison.OrdinalIgnoreCase)
                           || item.Statut.Contains(filters.SearchTerm, StringComparison.OrdinalIgnoreCase))
            .Where(item => string.IsNullOrWhiteSpace(filters.Status)
                           || string.Equals(item.Statut, filters.Status, StringComparison.OrdinalIgnoreCase))
            .Where(item => string.IsNullOrWhiteSpace(filters.Ville)
                           || item.Ville.Contains(filters.Ville, StringComparison.OrdinalIgnoreCase))
            .Where(item => !filters.DateLivraison.HasValue
                           || item.DateLivraison.Date == filters.DateLivraison.Value.Date)
            .OrderByDescending(item => item.DateLivraison)
            .ToList();
    }
}
