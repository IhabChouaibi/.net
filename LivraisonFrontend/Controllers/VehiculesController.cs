using LivraisonFrontend.Helpers;
using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Controllers;

public class VehiculesController : CrudControllerBase<VehiculeModel, IVehiculeApiService>
{
    public VehiculesController(IVehiculeApiService service, SessionManager sessionManager)
        : base(service, sessionManager)
    {
    }

    protected override string ModuleName => "Véhicules";

    protected override string SearchPlaceholder => "Rechercher un véhicule par immatriculation, marque ou modèle...";
}
