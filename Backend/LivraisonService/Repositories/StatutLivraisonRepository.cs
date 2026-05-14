using LivraisonService.Data;
using LivraisonService.Interfaces;
using LivraisonService.Models;

namespace LivraisonService.Repositories;

public class StatutLivraisonRepository : GenericRepository<StatutLivraison>, IStatutLivraisonRepository
{
    public StatutLivraisonRepository(LivraisonDbContext context)
        : base(context)
    {
    }
}
