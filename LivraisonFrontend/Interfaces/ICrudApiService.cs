using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Interfaces;

public interface ICrudApiService<TModel>
{
    Task<PagedResult<TModel>> GetPagedAsync(SearchFilterViewModel filters);

    Task<TModel?> GetByIdAsync(string id);

    Task CreateAsync(TModel model);

    Task UpdateAsync(string id, TModel model);

    Task DeleteAsync(string id);
}
