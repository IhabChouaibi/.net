using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Controllers;

public class LivreursController : CrudControllerBase<LivreurModel, ILivreurApiService>
{
    public LivreursController(ILivreurApiService service, SessionManager sessionManager)
        : base(service, sessionManager)
    {
    }

    protected override string ModuleName => "Livreurs";

    protected override string SearchPlaceholder => "Rechercher un livreur par nom, zone ou téléphone...";
}
