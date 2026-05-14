using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Services;

public class PaiementApiService : CrudApiServiceBase<PaiementModel>, IPaiementApiService
{
    public PaiementApiService(IHttpClientFactory httpClientFactory, ILogger<PaiementApiService> logger)
        : base(httpClientFactory, logger, "/paiements")
    {
    }
}
