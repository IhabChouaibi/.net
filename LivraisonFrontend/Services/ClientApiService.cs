using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Services;

public class ClientApiService : CrudApiServiceBase<ClientModel>, IClientApiService
{
    public ClientApiService(IHttpClientFactory httpClientFactory, ILogger<ClientApiService> logger)
        : base(httpClientFactory, logger, "/clients")
    {
    }

    public Task<ClientModel> RegisterAsync(ClientModel model) => PostAsync<ClientModel, ClientModel>("/clients/register", model);

    public Task<ClientModel?> GetByCompteIdAsync(int compteId) => GetOptionalAsync<ClientModel>($"/clients/by-compte/{compteId}");

    public Task UpdateByCompteIdAsync(int compteId, ClientModel model) => PutAsync($"/clients/by-compte/{compteId}", model);
}
