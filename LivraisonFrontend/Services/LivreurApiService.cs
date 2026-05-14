using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Services;

public class LivreurApiService : CrudApiServiceBase<LivreurModel>, ILivreurApiService
{
    public LivreurApiService(IHttpClientFactory httpClientFactory, ILogger<LivreurApiService> logger)
        : base(httpClientFactory, logger, "/livreurs")
    {
    }
}
