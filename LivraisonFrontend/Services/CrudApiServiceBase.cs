using LivraisonFrontend.Interfaces;
using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Services;

public abstract class CrudApiServiceBase<TModel> : ApiServiceBase, ICrudApiService<TModel>
{
    private readonly string _route;

    protected CrudApiServiceBase(IHttpClientFactory httpClientFactory, ILogger logger, string route)
        : base(httpClientFactory, logger)
    {
        _route = route;
    }

    public virtual Task<LivraisonFrontend.Models.PagedResult<TModel>> GetPagedAsync(SearchFilterViewModel filters)
        => GetPagedAsync<TModel>($"{_route}{BuildQueryString(filters)}", filters);

    public virtual Task<TModel?> GetByIdAsync(string id)
        => GetAsync<TModel>($"{_route}/{id}");

    public virtual Task CreateAsync(TModel model)
        => PostAsync(_route, model);

    public virtual Task UpdateAsync(string id, TModel model)
        => PutAsync($"{_route}/{id}", model);

    public virtual new Task DeleteAsync(string id)
        => base.DeleteAsync($"{_route}/{id}");
}
