namespace LivraisonFrontend.Models;

public class ApiSettings
{
    public const string SectionName = "ApiSettings";

    public string GatewayBaseUrl { get; set; } = "http://localhost:5000";
}
