using PaiementService.Helpers;
using PaiementService.Interfaces;
using PaiementService.Models;

namespace PaiementService.Services;

public class PaiementCrudService : IPaiementService
{
    private readonly IPaiementRepository _paiementRepository;

    public PaiementCrudService(IPaiementRepository paiementRepository)
    {
        _paiementRepository = paiementRepository;
    }

    public async Task<PagedResult<Paiement>> GetPagedAsync(SearchRequest request) =>
        await _paiementRepository.GetPagedAsync(request.SearchTerm, request.SortBy, request.SortDirection, request.Page, request.PageSize);

    public async Task<IEnumerable<Paiement>> GetAllAsync() => await _paiementRepository.GetAllAsync();

    public async Task<Paiement?> GetByIdAsync(int id) => await _paiementRepository.GetByIdAsync(id);

    public async Task<Paiement> AddAsync(Paiement paiement)
    {
        await _paiementRepository.AddAsync(paiement);
        await _paiementRepository.SaveAsync();
        return paiement;
    }

    public async Task<bool> UpdateAsync(int id, Paiement paiement)
    {
        var existingPaiement = await _paiementRepository.GetByIdAsync(id);
        if (existingPaiement is null)
        {
            return false;
        }

        existingPaiement.Montant = paiement.Montant;
        existingPaiement.DatePaiement = paiement.DatePaiement;
        existingPaiement.ModePaiement = paiement.ModePaiement;
        existingPaiement.ColisId = paiement.ColisId;

        _paiementRepository.Update(existingPaiement);
        await _paiementRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingPaiement = await _paiementRepository.GetByIdAsync(id);
        if (existingPaiement is null)
        {
            return false;
        }

        _paiementRepository.Delete(existingPaiement);
        await _paiementRepository.SaveAsync();
        return true;
    }
}
