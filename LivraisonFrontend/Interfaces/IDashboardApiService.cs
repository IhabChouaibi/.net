using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Interfaces;

public interface IDashboardApiService
{
    Task<DashboardViewModel> GetDashboardAsync();
}
