using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public class DashboardController : AppController
{
    private readonly IDashboardApiService _dashboardApiService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IDashboardApiService dashboardApiService,
        SessionManager sessionManager,
        ILogger<DashboardController> logger)
        : base(sessionManager)
    {
        _dashboardApiService = dashboardApiService;
        _logger = logger;
    }

    public Task<IActionResult> Index() =>
        ExecuteAsync(async () =>
        {
            var token = HttpContext.Session.GetString("Token");
            var role = HttpContext.Session.GetString("Role");
            Console.WriteLine($"TOKEN={(string.IsNullOrWhiteSpace(token) ? "(null)" : "present")}");
            Console.WriteLine($"ROLE={role ?? "(null)"}");
            _logger.LogInformation(
                "Dashboard/Index. Token present: {HasToken}. Role: {Role}. Controller: Dashboard. Action: Index.",
                !string.IsNullOrWhiteSpace(token),
                role ?? "(null)");

            var accessRedirect = RequireAdminAccess();
            if (accessRedirect is not null)
            {
                if (accessRedirect is RedirectToActionResult redirect)
                {
                    Console.WriteLine($"REDIRECT TO={redirect.ControllerName}/{redirect.ActionName}");
                }
                return accessRedirect;
            }

            Console.WriteLine("REDIRECT TO=(none)");
            ViewData["Title"] = "Dashboard";
            var dashboard = await _dashboardApiService.GetDashboardAsync();
            return View(dashboard);
        }, fallbackAction: "Login", fallbackController: "Auth");
}
