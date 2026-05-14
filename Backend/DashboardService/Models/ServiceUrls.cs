namespace DashboardService.Models;

public class ServiceUrls
{
    public const string SectionName = "ServiceUrls";

    public string ClientService { get; set; } = string.Empty;
    public string LivreurService { get; set; } = string.Empty;
    public string ColisService { get; set; } = string.Empty;
    public string PaiementService { get; set; } = string.Empty;
}
