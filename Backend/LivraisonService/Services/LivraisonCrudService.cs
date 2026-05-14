using LivraisonService.Helpers;
using LivraisonService.Interfaces;
using LivraisonService.Models;

namespace LivraisonService.Services;

public class LivraisonCrudService : ILivraisonService
{
    private readonly IAdresseLivraisonRepository _adresseRepository;
    private readonly IStatutLivraisonRepository _statutRepository;

    public LivraisonCrudService(IAdresseLivraisonRepository adresseRepository, IStatutLivraisonRepository statutRepository)
    {
        _adresseRepository = adresseRepository;
        _statutRepository = statutRepository;
    }

    public async Task<PagedResult<AdresseLivraison>> GetPagedAsync(SearchRequest request) =>
        await _adresseRepository.GetPagedAsync(request.SearchTerm, request.SortBy, request.SortDirection, request.Page, request.PageSize);

    public async Task<IEnumerable<AdresseLivraison>> GetAllAsync() => await _adresseRepository.GetAllAsync();
    public async Task<AdresseLivraison?> GetByIdAsync(int id) => await _adresseRepository.GetByIdAsync(id);

    public async Task<AdresseLivraison> AddAsync(AdresseLivraison adresseLivraison)
    {
        await _adresseRepository.AddAsync(adresseLivraison);
        await _adresseRepository.SaveAsync();
        return adresseLivraison;
    }

    public async Task<bool> UpdateAsync(int id, AdresseLivraison adresseLivraison)
    {
        var existingAdresse = await _adresseRepository.GetByIdAsync(id);
        if (existingAdresse is null)
        {
            return false;
        }

        existingAdresse.Adresse = adresseLivraison.Adresse;
        existingAdresse.Ville = adresseLivraison.Ville;
        existingAdresse.CodePostal = adresseLivraison.CodePostal;
        existingAdresse.ColisId = adresseLivraison.ColisId;

        _adresseRepository.Update(existingAdresse);
        await _adresseRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingAdresse = await _adresseRepository.GetByIdAsync(id);
        if (existingAdresse is null)
        {
            return false;
        }

        _adresseRepository.Delete(existingAdresse);
        await _adresseRepository.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<StatutLivraison>> GetStatutsAsync() => await _statutRepository.GetAllAsync();

    public async Task<StatutLivraison?> GetStatutByIdAsync(int id) => await _statutRepository.GetByIdAsync(id);

    public async Task<StatutLivraison> AddStatutAsync(StatutLivraison statutLivraison)
    {
        await _statutRepository.AddAsync(statutLivraison);
        await _statutRepository.SaveAsync();
        return statutLivraison;
    }

    public async Task<bool> UpdateStatutAsync(int id, StatutLivraison statutLivraison)
    {
        var existingStatut = await _statutRepository.GetByIdAsync(id);
        if (existingStatut is null)
        {
            return false;
        }

        existingStatut.Libelle = statutLivraison.Libelle;
        _statutRepository.Update(existingStatut);
        await _statutRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteStatutAsync(int id)
    {
        var existingStatut = await _statutRepository.GetByIdAsync(id);
        if (existingStatut is null)
        {
            return false;
        }

        _statutRepository.Delete(existingStatut);
        await _statutRepository.SaveAsync();
        return true;
    }
}
