using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public class AccountsController : AppController
{
    private readonly IAuthApiService _authApiService;
    private readonly IClientApiService _clientApiService;

    public AccountsController(
        IAuthApiService authApiService,
        IClientApiService clientApiService,
        SessionManager sessionManager)
        : base(sessionManager)
    {
        _authApiService = authApiService;
        _clientApiService = clientApiService;
    }

    public Task<IActionResult> Index(SearchFilterViewModel filters) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Comptes";
            var data = await _authApiService.GetAccountsAsync(filters);
            return View(new CrudPageViewModel<AccountModel>
            {
                ModuleName = "Comptes",
                SearchPlaceholder = "Rechercher un compte par nom, login, email ou rôle...",
                CreateActionLabel = "Nouveau compte",
                Filters = filters,
                Data = data
            });
        });

    public Task<IActionResult> Details(int id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Détails compte";
            var model = await _authApiService.GetAccountByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(model);
        });

    public IActionResult Create()
    {
        var accessRedirect = RequireAdminAccess();
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        ViewData["Title"] = "Créer un compte";
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Create(RegisterViewModel viewModel) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var registerResponse = await _authApiService.RegisterAsync(new RegisterRequest
            {
                Nom = viewModel.Nom,
                Prenom = viewModel.Prenom,
                Email = viewModel.Email,
                Telephone = viewModel.Telephone,
                CIN = viewModel.CIN,
                Adresse = viewModel.Adresse,
                Ville = viewModel.Ville,
                CodePostal = viewModel.CodePostal,
                Login = viewModel.Login,
                Password = viewModel.Password,
                ConfirmPassword = viewModel.ConfirmPassword
            });
            await _clientApiService.RegisterAsync(new ClientModel
            {
                CompteId = registerResponse.CompteId ?? registerResponse.UserId ?? registerResponse.EffectiveUserId,
                Nom = viewModel.Nom,
                Prenom = viewModel.Prenom,
                Email = viewModel.Email,
                Telephone = viewModel.Telephone,
                CIN = viewModel.CIN,
                Adresse = viewModel.Adresse,
                Ville = viewModel.Ville,
                CodePostal = viewModel.CodePostal
            });

            ShowSuccess("Compte créé avec succès.");
            return RedirectToAction(nameof(Index));
        });

    public Task<IActionResult> Edit(int id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Modifier un compte";
            var model = await _authApiService.GetAccountByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(model);
        });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(int id, AccountModel model) =>
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

            await _authApiService.UpdateAccountAsync(id, model);
            ShowSuccess("Compte mis à jour avec succès.");
            return RedirectToAction(nameof(Index));
        });

    public Task<IActionResult> Delete(int id) =>
        ExecuteAsync(async () =>
        {
            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                return accessRedirect;
            }

            ViewData["Title"] = "Supprimer un compte";
            var model = await _authApiService.GetAccountByIdAsync(id);
            if (model is null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(model);
        });

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteConfirmed(int id) =>
        ExecuteAsync(async () =>
        {
            await _authApiService.DeleteAccountAsync(id);
            ShowSuccess("Compte supprimé avec succès.");
            return RedirectToAction(nameof(Index));
        });
}
