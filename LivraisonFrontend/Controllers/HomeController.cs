using Microsoft.AspNetCore.Mvc;

namespace LivraisonFrontend.Controllers;

public class HomeController : Controller
{
    public IActionResult AccessDenied() => View();
}
