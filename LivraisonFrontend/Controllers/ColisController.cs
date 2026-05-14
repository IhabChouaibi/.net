using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public class ColisController : AppController
{
    private readonly IColisApiService _colisApiService;

    public ColisController(IColisApiService colisApiService, SessionManager sessionManager)
        : base(sessionManager)
    {
        _colisApiService = colisApiService;
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

    public IActionResult Create()
    {
        var accessRedirect = RequireAdminAccess();
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        ViewData["Title"] = "Créer un colis";
        return View(new ColisModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Create(ColisModel model) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _colisApiService.CreateAsync(model);
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

            return View(model);
        });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(string id, ColisModel model) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _colisApiService.UpdateAsync(id, model);
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
}
