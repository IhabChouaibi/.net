namespace AuthService.Helpers;

public class SearchRequest
{
    public string SearchTerm { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
