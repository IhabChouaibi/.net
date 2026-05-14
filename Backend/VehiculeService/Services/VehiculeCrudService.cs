using VehiculeService.Factories;
using VehiculeService.Helpers;
using VehiculeService.Interfaces;
using VehiculeService.Models;

namespace VehiculeService.Services;

public class VehiculeCrudService : IVehiculeService
{
    private readonly IVehiculeFactory _vehiculeFactory;
    private readonly IVehiculeRepository _vehiculeRepository;

    public VehiculeCrudService(IVehiculeRepository vehiculeRepository, IVehiculeFactory vehiculeFactory)
    {
        _vehiculeRepository = vehiculeRepository;
        _vehiculeFactory = vehiculeFactory;
    }

    public async Task<PagedResult<Vehicule>> GetPagedAsync(SearchRequest request) =>
        await _vehiculeRepository.GetPagedAsync(request.SearchTerm, request.SortBy, request.SortDirection, request.Page, request.PageSize);

    public async Task<IEnumerable<Vehicule>> GetAllAsync() => await _vehiculeRepository.GetAllAsync();

    public async Task<Vehicule?> GetByIdAsync(int id) => await _vehiculeRepository.GetByIdAsync(id);

    public async Task<Vehicule> AddAsync(VehiculeRequest request)
    {
        var vehicule = BuildVehiculeFromRequest(request);
        await _vehiculeRepository.AddAsync(vehicule);
        await _vehiculeRepository.SaveAsync();
        return vehicule;
    }

    public async Task<bool> UpdateAsync(int id, VehiculeRequest request)
    {
        var existingVehicule = await _vehiculeRepository.GetByIdAsync(id);
        if (existingVehicule is null)
        {
            return false;
        }

        if (!string.Equals(existingVehicule.Type, request.Type, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        existingVehicule.Couleur = request.Couleur;
        existingVehicule.Marque = request.Marque;
        existingVehicule.Matricule = request.Matricule;
        existingVehicule.VitesseLimite = request.VitesseLimite;

        if (existingVehicule is Camion camion)
        {
            camion.Capacite = request.Capacite;
            camion.NbrEssieux = request.NbrEssieux;
        }

        if (existingVehicule is Voiture voiture)
        {
            voiture.NbrPlaces = request.NbrPlaces;
        }

        _vehiculeRepository.Update(existingVehicule);
        await _vehiculeRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicule = await _vehiculeRepository.GetByIdAsync(id);
        if (vehicule is null)
        {
            return false;
        }

        _vehiculeRepository.Delete(vehicule);
        await _vehiculeRepository.SaveAsync();
        return true;
    }

    private Vehicule BuildVehiculeFromRequest(VehiculeRequest request)
    {
        var vehicule = _vehiculeFactory.CreateVehicule(request.Type);
        vehicule.Couleur = request.Couleur;
        vehicule.Marque = request.Marque;
        vehicule.Matricule = request.Matricule;
        vehicule.VitesseLimite = request.VitesseLimite;

        if (vehicule is Camion camion)
        {
            camion.Capacite = request.Capacite;
            camion.NbrEssieux = request.NbrEssieux;
        }

        if (vehicule is Voiture voiture)
        {
            voiture.NbrPlaces = request.NbrPlaces;
        }

        return vehicule;
    }
}
