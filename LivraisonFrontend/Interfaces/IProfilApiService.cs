using LivraisonFrontend.Models;

namespace LivraisonFrontend.Interfaces;

public interface IProfilApiService
{
    Task<ClientModel?> GetProfileAsync(int compteId);
    Task UpdateProfileAsync(int compteId, ClientModel model);
}
