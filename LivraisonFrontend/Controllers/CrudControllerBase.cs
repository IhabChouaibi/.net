using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public abstract class CrudControllerBase<TModel, TService> : AppController
    where TModel : class, new()
    where TService : ICrudApiService<TModel>
{
    protected readonly TService Service;

    protected CrudControllerBase(TService service, SessionManager sessionManager)
        : base(sessionManager)
    {
        Service = service;
    }

    protected abstract string ModuleName { get; }

    protected virtual string SearchPlaceholder => $"Rechercher dans {ModuleName.ToLowerInvariant()}...";

    protected virtual string CreateActionLabel => $"Nouveau {ModuleName[..^1]}";

    public virtual Task<IActionResult> Index(SearchFilterViewModel filters) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = ModuleName;
            var data = await Service.GetPagedAsync(filters);
            return View(new CrudPageViewModel<TModel>
            {
                ModuleName = ModuleName,
                SearchPlaceholder = SearchPlaceholder,
                CreateActionLabel = CreateActionLabel,
                ReadOnly = !IsAdmin,
                Filters = filters,
                Data = data
            });
        });

    public virtual Task<IActionResult> Details(string id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = $"Détails {ModuleName}";
            var model = await Service.GetByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(model);
        });

    public virtual IActionResult Create()
    {
        var accessRedirect = RequireAdminAccess();
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        ViewData["Title"] = $"Créer {ModuleName}";
        return View(new TModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual Task<IActionResult> Create(TModel model) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = $"Créer {ModuleName}";
                return View(model);
            }

            await Service.CreateAsync(model);
            ShowSuccess($"{ModuleName} ajouté avec succès.");
            return RedirectToAction(nameof(Index));
        });

    public virtual Task<IActionResult> Edit(string id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = $"Modifier {ModuleName}";
            var model = await Service.GetByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(model);
        });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual Task<IActionResult> Edit(string id, TModel model) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = $"Modifier {ModuleName}";
                return View(model);
            }

            await Service.UpdateAsync(id, model);
            ShowSuccess($"{ModuleName} mis à jour avec succès.");
            return RedirectToAction(nameof(Index));
        });

    public virtual Task<IActionResult> Delete(string id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = $"Supprimer {ModuleName}";
            var model = await Service.GetByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(model);
        });

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public virtual Task<IActionResult> DeleteConfirmed(string id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            await Service.DeleteAsync(id);
            ShowSuccess($"{ModuleName} supprimé avec succès.");
            return RedirectToAction(nameof(Index));
        });
}
