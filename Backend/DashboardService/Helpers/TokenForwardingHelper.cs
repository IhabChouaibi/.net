namespace DashboardService.Helpers;

public static class TokenForwardingHelper
{
    public static string ExtractBearerToken(HttpRequest request)
    {
        var header = request.Headers.Authorization.ToString();
        return header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? header["Bearer ".Length..]
            : string.Empty;
    }
}
