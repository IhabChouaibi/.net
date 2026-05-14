using LivraisonFrontend.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public abstract class AppController : Controller
{
    protected readonly SessionManager SessionManager;

    protected AppController(SessionManager sessionManager)
    {
        SessionManager = sessionManager;
    }

    protected void ShowSuccess(string message) => SetNotification("success", "Succès", message);

    protected void ShowError(string message) => SetNotification("error", "Erreur", message);

    protected void ShowInfo(string message) => SetNotification("info", "Information", message);

    protected bool IsAdmin => string.Equals(SessionManager.Role, "Admin", StringComparison.OrdinalIgnoreCase);

    protected bool IsUser => string.Equals(SessionManager.Role, "User", StringComparison.OrdinalIgnoreCase);

    protected IActionResult? RequireAuthenticatedAccess()
    {
        if (SessionManager.IsAuthenticated)
        {
            return null;
        }

        return RedirectToAction("Login", "Auth");
    }

    protected IActionResult? RequireAdminAccess()
    {
        var authRedirect = RequireAuthenticatedAccess();
        if (authRedirect is not null)
        {
            return authRedirect;
        }

        return IsAdmin ? null : RedirectToAction("AccessDenied", "Home");
    }

    protected IActionResult? RequireUserAccess()
    {
        var authRedirect = RequireAuthenticatedAccess();
        if (authRedirect is not null)
        {
            return authRedirect;
        }

        if (IsAdmin)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return IsUser ? null : RedirectToAction("AccessDenied", "Home");
    }

    protected async Task<IActionResult> ExecuteAsync(
        Func<Task<IActionResult>> action,
        string fallbackAction = "Index",
        string fallbackController = "",
        object? routeValues = null)
    {
        try
        {
            return await action();
        }
        catch (UnauthorizedAccessException exception)
        {
            await ClearAuthenticationAsync();
            ShowError(exception.Message);
            return RedirectToAction("Login", "Auth");
        }
        catch (ApplicationException exception)
        {
            ShowError(exception.Message);

            if (SessionManager.IsAuthenticated
                && string.Equals(fallbackAction, "Login", StringComparison.OrdinalIgnoreCase)
                && string.Equals(fallbackController, "Auth", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Error");
            }

            return string.IsNullOrWhiteSpace(fallbackController)
                ? RedirectToAction(fallbackAction, routeValues)
                : RedirectToAction(fallbackAction, fallbackController, routeValues);
        }
        catch (Exception)
        {
            ShowError("Une erreur inattendue s'est produite.");
            return RedirectToAction("Index", "Error");
        }
    }

    protected async Task ClearAuthenticationAsync()
    {
        SessionManager.Clear();
        await Task.CompletedTask;
    }

    private void SetNotification(string type, string title, string message)
    {
        TempData["Notification.Type"] = type;
        TempData["Notification.Title"] = title;
        TempData["Notification.Message"] = message;
    }
}
