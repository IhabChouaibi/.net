using LivreurService.Helpers;
using LivreurService.Interfaces;
using LivreurService.Models;

namespace LivreurService.Services;

public class LivreurCrudService : ILivreurService
{
    private readonly ILivreurRepository _livreurRepository;

    public LivreurCrudService(ILivreurRepository livreurRepository)
    {
        _livreurRepository = livreurRepository;
    }

    public async Task<PagedResult<Livreur>> GetPagedAsync(SearchRequest request) =>
        await _livreurRepository.GetPagedAsync(request.SearchTerm, request.SortBy, request.SortDirection, request.Page, request.PageSize);

    public async Task<IEnumerable<Livreur>> GetAllAsync() => await _livreurRepository.GetAllAsync();

    public async Task<Livreur?> GetByIdAsync(int id) => await _livreurRepository.GetByIdAsync(id);

    public async Task<Livreur> AddAsync(Livreur livreur)
    {
        await _livreurRepository.AddAsync(livreur);
        await _livreurRepository.SaveAsync();
        return livreur;
    }

    public async Task<bool> UpdateAsync(int id, Livreur livreur)
    {
        var existingLivreur = await _livreurRepository.GetByIdAsync(id);
        if (existingLivreur is null)
        {
            return false;
        }

        existingLivreur.CIN = livreur.CIN;
        existingLivreur.RaisonSocial = livreur.RaisonSocial;
        existingLivreur.Ville = livreur.Ville;
        existingLivreur.CodePostal = livreur.CodePostal;

        _livreurRepository.Update(existingLivreur);
        await _livreurRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingLivreur = await _livreurRepository.GetByIdAsync(id);
        if (existingLivreur is null)
        {
            return false;
        }

        _livreurRepository.Delete(existingLivreur);
        await _livreurRepository.SaveAsync();
        return true;
    }
}
