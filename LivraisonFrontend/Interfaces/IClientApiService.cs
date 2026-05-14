using LivraisonFrontend.Models;

namespace LivraisonFrontend.Interfaces;

public interface IClientApiService : ICrudApiService<ClientModel>
{
    Task<ClientModel> RegisterAsync(ClientModel model);
    Task<ClientModel?> GetByCompteIdAsync(int compteId);
    Task UpdateByCompteIdAsync(int compteId, ClientModel model);
}
