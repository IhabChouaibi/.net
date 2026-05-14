namespace VehiculeService.Helpers;

public class SearchRequest
{
    public string SearchTerm { get; set; } = string.Empty;
    public string SortBy { get; set; } = "Marque";
    public string SortDirection { get; set; } = "asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
