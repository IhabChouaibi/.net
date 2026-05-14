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
        var accessRedirect = RequireUserAccess();
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        ViewData["Title"] = "Mon profil";
        var profile = await _profilApiService.GetProfileAsync(SessionManager.UserId);
        if (profile is null)
        {
            ShowError("Aucun profil client n'est lié à ce compte.");
            return RedirectToAction("Login", "Auth");
        }

        return View(MapToViewModel(profile));
    }, fallbackAction: "Login", fallbackController: "Auth");

    public Task<IActionResult> Edit() => ExecuteAsync(async () =>
    {
        var accessRedirect = RequireUserAccess();
        if (accessRedirect is not null)
        {
            return accessRedirect;
        }

        ViewData["Title"] = "Modifier mon profil";
        var profile = await _profilApiService.GetProfileAsync(SessionManager.UserId);
        if (profile is null)
        {
            ShowError("Aucun profil client n'est lié à ce compte.");
            return RedirectToAction(nameof(Index));
        }

        return View(MapToViewModel(profile));
    }, fallbackAction: "Login", fallbackController: "Auth");

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(ProfilViewModel viewModel) => ExecuteAsync(async () =>
    {
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

        await _profilApiService.UpdateProfileAsync(SessionManager.UserId, new ClientModel
        {
            CompteId = SessionManager.UserId,
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
