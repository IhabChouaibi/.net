using VehiculeService.Models;

namespace VehiculeService.Interfaces;

public interface IVehiculeRepository : IGenericRepository<Vehicule>
{
    Task<PagedResult<Vehicule>> GetPagedAsync(string searchTerm, string sortBy, string sortDirection, int page, int pageSize);
}
