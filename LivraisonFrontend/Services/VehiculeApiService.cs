using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Services;

public class VehiculeApiService : CrudApiServiceBase<VehiculeModel>, IVehiculeApiService
{
    public VehiculeApiService(IHttpClientFactory httpClientFactory, ILogger<VehiculeApiService> logger)
        : base(httpClientFactory, logger, "/vehicules")
    {
    }
}
