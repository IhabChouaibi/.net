using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LivraisonFrontend.Controllers;

public class ColisController : AppController
{
    private readonly IColisApiService _colisApiService;
    private readonly IClientApiService _clientApiService;
    private readonly ILivreurApiService _livreurApiService;

    public ColisController(
        IColisApiService colisApiService,
        IClientApiService clientApiService,
        ILivreurApiService livreurApiService,
        SessionManager sessionManager)
        : base(sessionManager)
    {
        _colisApiService = colisApiService;
        _clientApiService = clientApiService;
        _livreurApiService = livreurApiService;
    }

    public Task<IActionResult> Index(SearchFilterViewModel filters) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Colis";
            var data = await _colisApiService.GetPagedAsync(filters);
            return View(new CrudPageViewModel<ColisModel>
            {
                ModuleName = "Colis",
                SearchPlaceholder = "Rechercher un colis par tracking, client ou statut...",
                CreateActionLabel = "Nouveau colis",
                ReadOnly = !IsAdmin,
                Filters = filters,
                Data = data
            });
        }, fallbackAction: "Login", fallbackController: "Auth");

    public Task<IActionResult> Details(string id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Détails colis";
            var model = await _colisApiService.GetByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(model);
        }, fallbackAction: "Index");

    public Task<IActionResult> Create() =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Créer un colis";
            return View(await BuildCreateViewModelAsync(new ColisModel()));
        }, fallbackAction: nameof(Index));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Create(CreateColisViewModel viewModel) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Créer un colis";
                return View(await BuildCreateViewModelAsync(viewModel.Colis));
            }

            await _colisApiService.CreateAsync(viewModel.Colis);
            ShowSuccess("Colis ajouté avec succès.");
            return RedirectToAction(nameof(Index));
        });

    public Task<IActionResult> Edit(string id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Modifier un colis";
            var model = await _colisApiService.GetByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(await BuildCreateViewModelAsync(model));
        });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(string id, CreateColisViewModel viewModel) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Modifier un colis";
                return View(await BuildCreateViewModelAsync(viewModel.Colis));
            }

            await _colisApiService.UpdateAsync(id, viewModel.Colis);
            ShowSuccess("Colis mis à jour avec succès.");
            return RedirectToAction(nameof(Index));
        });

    public Task<IActionResult> Delete(string id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Supprimer un colis";
            var model = await _colisApiService.GetByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(model);
        });

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteConfirmed(string id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            await _colisApiService.DeleteAsync(id);
            ShowSuccess("Colis supprimé avec succès.");
            return RedirectToAction(nameof(Index));
        });

    private async Task<CreateColisViewModel> BuildCreateViewModelAsync(ColisModel colis)
    {
        // Load lookup data once per view so the UI shows readable names while the API still receives IDs.
        var filters = new SearchFilterViewModel { Page = 1, PageSize = 500 };
        var clients = await _clientApiService.GetPagedAsync(filters);
        var livreurs = await _livreurApiService.GetPagedAsync(filters);

        return new CreateColisViewModel
        {
            Colis = colis,
            ListeClients = clients.Items
                .OrderBy(item => item.Nom)
                .ThenBy(item => item.Prenom)
                .Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = $"{item.Nom} {item.Prenom}".Trim()
                }),
            ListeLivreurs = livreurs.Items
                .OrderBy(item => item.RaisonSocial)
                .Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.RaisonSocial
                })
        };
    }
}
