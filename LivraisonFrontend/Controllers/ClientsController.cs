using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Controllers;

public class ClientsController : CrudControllerBase<ClientModel, IClientApiService>
{
    public ClientsController(IClientApiService service, SessionManager sessionManager)
        : base(service, sessionManager)
    {
    }

    protected override string ModuleName => "Clients";

    protected override string SearchPlaceholder => "Rechercher un client par nom, email ou téléphone...";
}
