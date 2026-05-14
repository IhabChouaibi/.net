using LivraisonFrontend.Interfaces;
using LivraisonFrontend.Models;

namespace LivraisonFrontend.Services;

public class ProfilApiService : IProfilApiService
{
    private readonly IClientApiService _clientApiService;

    public ProfilApiService(IClientApiService clientApiService)
    {
        _clientApiService = clientApiService;
    }

    public Task<ClientModel?> GetProfileAsync(int compteId) => _clientApiService.GetByCompteIdAsync(compteId);

    public Task UpdateProfileAsync(int compteId, ClientModel model) => _clientApiService.UpdateByCompteIdAsync(compteId, model);
}
