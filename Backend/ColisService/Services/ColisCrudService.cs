using ColisService.Helpers;
using ColisService.Interfaces;
using ColisService.Models;

namespace ColisService.Services;

public class ColisCrudService : IColisService
{
    private readonly IColisRepository _colisRepository;

    public ColisCrudService(IColisRepository colisRepository)
    {
        _colisRepository = colisRepository;
    }

    public async Task<PagedResult<Colis>> GetPagedAsync(SearchRequest request) =>
        await _colisRepository.GetPagedAsync(request.SearchTerm, request.Status, request.SortBy, request.SortDirection, request.Page, request.PageSize);

    public async Task<IEnumerable<Colis>> GetAllAsync() => await _colisRepository.GetAllAsync();

    public async Task<IEnumerable<Colis>> GetByClientIdAsync(int clientId) => await _colisRepository.GetByClientIdAsync(clientId);

    public async Task<Colis?> GetByIdAsync(int id) => await _colisRepository.GetByIdAsync(id);

    public async Task<Colis> AddAsync(Colis colis)
    {
        await _colisRepository.AddAsync(colis);
        await _colisRepository.SaveAsync();
        return colis;
    }

    public async Task<bool> UpdateAsync(int id, Colis colis)
    {
        var existingColis = await _colisRepository.GetByIdAsync(id);
        if (existingColis is null)
        {
            return false;
        }

        existingColis.Libelle = colis.Libelle;
        existingColis.DateLivraison = colis.DateLivraison;
        existingColis.Montant = colis.Montant;
        existingColis.Poids = colis.Poids;
        existingColis.Volume = colis.Volume;
        existingColis.ClientId = colis.ClientId;
        existingColis.LivreurId = colis.LivreurId;
        existingColis.StatutLivraisonId = colis.StatutLivraisonId;

        _colisRepository.Update(existingColis);
        await _colisRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingColis = await _colisRepository.GetByIdAsync(id);
        if (existingColis is null)
        {
            return false;
        }

        _colisRepository.Delete(existingColis);
        await _colisRepository.SaveAsync();
        return true;
    }
}
