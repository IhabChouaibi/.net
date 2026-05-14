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
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthApiService authApiService,
        IClientApiService clientApiService,
        SessionManager sessionManager,
        ILogger<AuthController> logger)
        : base(sessionManager)
    {
        _authApiService = authApiService;
        _clientApiService = clientApiService;
        _logger = logger;
    }

    public IActionResult Login()
    {
        var token = HttpContext.Session.GetString("Token");
        var role = HttpContext.Session.GetString("Role");

        Console.WriteLine($"TOKEN={(string.IsNullOrWhiteSpace(token) ? "(null)" : "present")}");
        Console.WriteLine($"ROLE={role ?? "(null)"}");

        _logger.LogInformation(
            "GET Login. Token present: {HasToken}. Role: {Role}. Controller: Auth. Action: Login.",
            !string.IsNullOrWhiteSpace(token),
            role ?? "(null)");

        if (!string.IsNullOrWhiteSpace(token) && string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            var redirectResult = RedirectToAction("Index", "Dashboard");
            Console.WriteLine($"REDIRECT TO={redirectResult.ControllerName}/{redirectResult.ActionName}");
            _logger.LogInformation(
                "GET Login redirect -> {Controller}/{Action}",
                redirectResult.ControllerName,
                redirectResult.ActionName);
            return redirectResult;
        }

        if (!string.IsNullOrWhiteSpace(token) && string.Equals(role, "User", StringComparison.OrdinalIgnoreCase))
        {
            var redirectResult = RedirectToAction("Index", "SuiviColis");
            Console.WriteLine($"REDIRECT TO={redirectResult.ControllerName}/{redirectResult.ActionName}");
            _logger.LogInformation(
                "GET Login redirect -> {Controller}/{Action}",
                redirectResult.ControllerName,
                redirectResult.ActionName);
            return redirectResult;
        }

        ViewData["Title"] = "Connexion";
        Console.WriteLine("REDIRECT TO=(none)");
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

            if (string.IsNullOrWhiteSpace(response.Login))
            {
                response.Login = viewModel.Login;
            }

            if (string.IsNullOrWhiteSpace(response.Role))
            {
                response.Role = "User";
            }

            if (string.IsNullOrWhiteSpace(response.FullName))
            {
                response.FullName = response.Login;
            }

            HttpContext.Session.SetString("Token", response.Token);
            HttpContext.Session.SetString("Role", response.Role);
            HttpContext.Session.SetString("Login", response.Login);
            HttpContext.Session.SetString("FullName", response.FullName);
            HttpContext.Session.SetString("UserId", response.Id.ToString());
            JwtSessionHelper.StoreUserSession(HttpContext.Session, response);

            var redirectResult = string.Equals(response.Role, "Admin", StringComparison.OrdinalIgnoreCase)
                ? RedirectToAction("Index", "Dashboard")
                : RedirectToAction("Index", "SuiviColis");
            Console.WriteLine($"TOKEN={(string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Token")) ? "(null)" : "present")}");
            Console.WriteLine($"ROLE={HttpContext.Session.GetString("Role") ?? "(null)"}");
            Console.WriteLine($"REDIRECT TO={redirectResult.ControllerName}/{redirectResult.ActionName}");
            _logger.LogInformation(
                "POST Login. Token present: {HasToken}. Role: {Role}. Controller: Auth. Action: Login. Redirect -> {Controller}/{Action}",
                !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Token")),
                HttpContext.Session.GetString("Role"),
                redirectResult.ControllerName,
                redirectResult.ActionName);
            _logger.LogInformation(
                "Login reussi pour {Login}. Role recu: {Role}. Host frontend: {Host}",
                response.Login,
                response.Role,
                Request.Host.Value);
            ShowSuccess("Connexion réussie.");
            return redirectResult;
        }
        catch (UnauthorizedAccessException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(viewModel);
        }
        catch (ApplicationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(viewModel);
        }
    }

    public IActionResult Register()
    {
        ViewData["Title"] = "Créer un compte";
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Créer un compte";
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
                ConfirmPassword = viewModel.ConfirmPassword
            });
            await _clientApiService.RegisterAsync(new ClientModel
            {
                CompteId = registerResponse.Id,
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
            return RedirectToAction(nameof(Login));
        }
        catch (ApplicationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(viewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await ClearAuthenticationAsync();
        _logger.LogInformation("Utilisateur deconnecte depuis le host frontend {Host}.", Request.Host.Value);
        ShowInfo("Vous êtes maintenant déconnecté.");
        return RedirectToAction(nameof(Login));
    }

}
