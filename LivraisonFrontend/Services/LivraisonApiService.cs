using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Services;

public class LivraisonApiService : CrudApiServiceBase<LivraisonModel>, ILivraisonApiService
{
    public LivraisonApiService(IHttpClientFactory httpClientFactory, ILogger<LivraisonApiService> logger)
        : base(httpClientFactory, logger, "/livraisons")
    {
    }

    public async Task<IReadOnlyList<LivraisonModel>> GetAllAsync()
    {
        var filters = new ViewModels.SearchFilterViewModel { Page = 1, PageSize = 500 };
        var result = await GetPagedAsync<LivraisonModel>("/livraisons/all", filters);
        return result.Items.ToList();
    }
}
