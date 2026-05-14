using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Services;

public class ColisApiService : CrudApiServiceBase<ColisModel>, IColisApiService
{
    public ColisApiService(IHttpClientFactory httpClientFactory, ILogger<ColisApiService> logger)
        : base(httpClientFactory, logger, "/colis")
    {
    }

    public async Task<IReadOnlyList<ColisModel>> GetByClientIdAsync(int clientId)
    {
        var filters = new ViewModels.SearchFilterViewModel { Page = 1, PageSize = 200 };
        var result = await GetPagedAsync<ColisModel>($"/colis/client/{clientId}", filters);
        return result.Items?.ToList() ?? new List<ColisModel>();
    }

    public async Task<IReadOnlyList<ColisModel>> GetAllAsync()
    {
        var filters = new ViewModels.SearchFilterViewModel { Page = 1, PageSize = 500 };
        var result = await GetPagedAsync<ColisModel>("/colis/all", filters);
        return result.Items?.ToList() ?? new List<ColisModel>();
    }
}
