using VehiculeService.Helpers;
using VehiculeService.Models;

namespace VehiculeService.Interfaces;

public interface IVehiculeService
{
    Task<PagedResult<Vehicule>> GetPagedAsync(SearchRequest request);
    Task<IEnumerable<Vehicule>> GetAllAsync();
    Task<Vehicule?> GetByIdAsync(int id);
    Task<Vehicule> AddAsync(VehiculeRequest request);
    Task<bool> UpdateAsync(int id, VehiculeRequest request);
    Task<bool> DeleteAsync(int id);
}
