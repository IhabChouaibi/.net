namespace LivraisonFrontend.ViewModels;

public class SearchFilterViewModel
{
    public string SearchTerm { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string SortBy { get; set; } = string.Empty;

    public string SortDirection { get; set; } = "desc";

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
