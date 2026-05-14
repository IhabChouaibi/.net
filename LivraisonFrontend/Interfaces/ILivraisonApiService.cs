using LivraisonFrontend.Models;

namespace LivraisonFrontend.Interfaces;

public interface ILivraisonApiService : ICrudApiService<LivraisonModel>
{
    Task<IReadOnlyList<LivraisonModel>> GetAllAsync();
}
