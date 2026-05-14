using LivraisonFrontend.Models;

namespace LivraisonFrontend.Interfaces;

public interface IColisApiService : ICrudApiService<ColisModel>
{
    Task<IReadOnlyList<ColisModel>> GetByClientIdAsync(int clientId);
}
