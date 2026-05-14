using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public class AuthController : AppController
{
    private readonly IAuthApiService _authApiService;
    private readonly IClientApiService _clientApiService;
    private readonly AuthSessionHelper _authSessionHelper;

    public AuthController(
        IAuthApiService authApiService,
        IClientApiService clientApiService,
        AuthSessionHelper authSessionHelper,
        SessionManager sessionManager)
        : base(sessionManager)
    {
        _authApiService = authApiService;
        _clientApiService = clientApiService;
        _authSessionHelper = authSessionHelper;
    }

    public IActionResult Login()
    {
        if (_authSessionHelper.IsAdmin())
        {
            return RedirectToAction("Index", "Dashboard");
        }

        if (_authSessionHelper.IsUser())
        {
            return RedirectToAction("Index", "ClientHome");
        }

        ViewData["Title"] = "Connexion";
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Connexion";
            return View(viewModel);
        }

        try
        {
            var response = await _authApiService.LoginAsync(new LoginRequest
            {
                Login = viewModel.Login,
                Password = viewModel.Password
            });

            response.Login = string.IsNullOrWhiteSpace(response.Login) ? viewModel.Login : response.Login;
            response.Role = NormalizeRole(response.Role);
            response.FullName = string.IsNullOrWhiteSpace(response.FullName)
                ? response.Login
                : response.FullName;

            if (!string.Equals(response.Role, "Admin", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(response.Role, "User", StringComparison.OrdinalIgnoreCase))
            {
                _authSessionHelper.ClearSession();
                return RedirectToAction("AccessDenied", "Home");
            }

            _authSessionHelper.SaveLoginSession(response);

            if (_authSessionHelper.IsUser() && !_authSessionHelper.GetClientId().HasValue)
            {
                var compteId = _authSessionHelper.GetCompteId() ?? _authSessionHelper.GetUserId();
                var resolvedCompteId = compteId.GetValueOrDefault();
                if (resolvedCompteId > 0)
                {
                    var client = await _clientApiService.GetByCompteIdAsync(resolvedCompteId);
                    if (client?.Id > 0)
                    {
                        _authSessionHelper.SetClientId(client.Id);
                    }
                }
            }

            ShowSuccess("Connexion réussie.");
            return _authSessionHelper.IsAdmin()
                ? RedirectToAction("Index", "Dashboard")
                : RedirectToAction("Index", "ClientHome");
        }
        catch (UnauthorizedAccessException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            ViewData["Title"] = "Connexion";
            return View(viewModel);
        }
        catch (ApplicationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            ViewData["Title"] = "Connexion";
            return View(viewModel);
        }
    }

    public IActionResult Register()
    {
        ViewData["Title"] = "Créer un compte client";
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Créer un compte client";
            return View(viewModel);
        }

        try
        {
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
                ConfirmPassword = viewModel.ConfirmPassword,
                Role = "User"
            });

            var compteId = registerResponse.CompteId ?? registerResponse.UserId ?? registerResponse.Id;
            var resolvedCompteId = compteId.GetValueOrDefault();
            if (resolvedCompteId <= 0)
            {
                throw new ApplicationException("Le compte a été créé sans identifiant exploitable.");
            }

            await _clientApiService.RegisterAsync(new ClientModel
            {
                CompteId = resolvedCompteId,
                Nom = viewModel.Nom,
                Prenom = viewModel.Prenom,
                Email = viewModel.Email,
                Telephone = viewModel.Telephone,
                CIN = viewModel.CIN,
                Adresse = viewModel.Adresse,
                Ville = viewModel.Ville,
                CodePostal = viewModel.CodePostal
            });

            ShowSuccess("Compte client créé avec succès. Connectez-vous pour continuer.");
            return RedirectToAction(nameof(Login));
        }
        catch (ApplicationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            ViewData["Title"] = "Créer un compte client";
            return View(viewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        _authSessionHelper.ClearSession();
        ShowInfo("Vous êtes maintenant déconnecté.");
        return RedirectToAction(nameof(Login));
    }

    private static string NormalizeRole(string? role)
    {
        if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return "Admin";
        }

        return "User";
    }
}
