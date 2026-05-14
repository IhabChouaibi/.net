using LivraisonFrontend.Models;

namespace LivraisonFrontend.ViewModels;

public class CrudPageViewModel<T>
{
    public string ModuleName { get; set; } = string.Empty;

    public string CreateActionLabel { get; set; } = "Nouveau";

    public string SearchPlaceholder { get; set; } = "Rechercher...";

    public bool ReadOnly { get; set; }

    public SearchFilterViewModel Filters { get; set; } = new();

    public PagedResult<T> Data { get; set; } = new();
}
