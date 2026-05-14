using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Controllers;

public class PaiementsController : CrudControllerBase<PaiementModel, IPaiementApiService>
{
    public PaiementsController(IPaiementApiService service, SessionManager sessionManager)
        : base(service, sessionManager)
    {
    }

    protected override string ModuleName => "Paiements";

    protected override string SearchPlaceholder => "Rechercher un paiement par référence, client ou statut...";
}
