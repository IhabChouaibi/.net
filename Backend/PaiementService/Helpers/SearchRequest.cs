namespace PaiementService.Helpers;

public class SearchRequest
{
    public string SearchTerm { get; set; } = string.Empty;
    public string SortBy { get; set; } = "DatePaiement";
    public string SortDirection { get; set; } = "desc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
