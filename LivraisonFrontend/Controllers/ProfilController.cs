using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public class ProfilController : AppController
{
    private readonly IProfilApiService _profilApiService;

    public ProfilController(IProfilApiService profilApiService, SessionManager sessionManager)
        : base(sessionManager)
    {
        _profilApiService = profilApiService;
    }

    public Task<IActionResult> Index() => ExecuteAsync(async () =>
    {
        if (string.IsNullOrWhiteSpace(SessionManager.GetToken()))
        {
            ShowError("Votre session a expiré. Veuillez vous reconnecter.");
            return RedirectToAction("Login", "Auth");
        }

        var accessRedirect = RequireUserAccess();
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        ViewData["Title"] = "Mon profil";
        var profile = await LoadProfileAsync();
        if (profile is null)
        {
            ShowError("Profil client introuvable.");
            return RedirectToAction("Index", "SuiviColis");
        }

        return View(MapToViewModel(profile));
    }, fallbackAction: "Login", fallbackController: "Auth");

    public Task<IActionResult> Edit() => ExecuteAsync(async () =>
    {
        if (string.IsNullOrWhiteSpace(SessionManager.GetToken()))
        {
            ShowError("Votre session a expiré. Veuillez vous reconnecter.");
            return RedirectToAction("Login", "Auth");
        }

        var accessRedirect = RequireUserAccess();
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        ViewData["Title"] = "Modifier mon profil";
        var profile = await LoadProfileAsync();
        if (profile is null)
        {
            ShowError("Profil client introuvable.");
            return RedirectToAction(nameof(Index));
        }

        return View(MapToViewModel(profile));
    }, fallbackAction: "Login", fallbackController: "Auth");

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(ProfilViewModel viewModel) => ExecuteAsync(async () =>
    {
        if (string.IsNullOrWhiteSpace(SessionManager.GetToken()))
        {
            ShowError("Votre session a expiré. Veuillez vous reconnecter.");
            return RedirectToAction("Login", "Auth");
        }

        var accessRedirect = RequireUserAccess();
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Modifier mon profil";
            return View(viewModel);
        }

        var profile = await LoadProfileAsync();
        if (profile is null)
        {
            ShowError("Impossible de retrouver le profil client associé à cette session.");
            return RedirectToAction(nameof(Index));
        }

        var compteId = SessionManager.GetCompteId() > 0 ? SessionManager.GetCompteId() : SessionManager.UserId;
        await _profilApiService.UpdateProfileAsync(compteId, new ClientModel
        {
            Id = profile.Id,
            CompteId = compteId,
            Nom = viewModel.Nom,
            Prenom = viewModel.Prenom,
            Email = viewModel.Email,
            Telephone = viewModel.Telephone,
            CIN = viewModel.CIN,
            Adresse = viewModel.Adresse,
            Ville = viewModel.Ville,
            CodePostal = viewModel.CodePostal
        });

        ShowSuccess("Profil mis à jour avec succès.");
        return RedirectToAction(nameof(Index));
    }, fallbackAction: nameof(Index));

    private async Task<ClientModel?> LoadProfileAsync()
    {
        var compteId = SessionManager.GetCompteId() > 0 ? SessionManager.GetCompteId() : SessionManager.GetUserId();
        if (compteId <= 0)
        {
            return null;
        }

        var profile = await _profilApiService.GetProfileAsync(compteId);
        if (profile?.Id > 0)
        {
            SessionManager.SetClientId(profile.Id);
        }

        return profile;
    }

    private ProfilViewModel MapToViewModel(ClientModel profile) => new()
    {
        CompteId = profile.CompteId,
        Nom = profile.Nom,
        Prenom = profile.Prenom,
        Email = profile.Email,
        Telephone = profile.Telephone,
        CIN = profile.CIN,
        Adresse = profile.Adresse,
        Ville = profile.Ville,
        CodePostal = profile.CodePostal,
        Login = SessionManager.Login
    };
}
