using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    [Route("Error")]
    public IActionResult Index() => View("Index");

    public IActionResult UnauthorizedPage() => View("Unauthorized");

    public IActionResult AccessDenied() => View();

    public IActionResult NotFoundPage() => View("NotFound");

    public IActionResult HandleStatusCode(int code)
    {
        return code switch
        {
            401 => RedirectToAction(nameof(UnauthorizedPage)),
            403 => RedirectToAction(nameof(AccessDenied)),
            404 => RedirectToAction(nameof(NotFoundPage)),
            _ => View("Index")
        };
    }
}
