using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Controllers;

public class LivraisonsController : CrudControllerBase<LivraisonModel, ILivraisonApiService>
{
    public LivraisonsController(ILivraisonApiService service, SessionManager sessionManager)
        : base(service, sessionManager)
    {
    }

    protected override string ModuleName => "Livraisons";

    protected override string SearchPlaceholder => "Rechercher une livraison par tracking, livreur ou statut...";
}
