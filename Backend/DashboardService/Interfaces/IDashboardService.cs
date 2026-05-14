using DashboardService.Models;

namespace DashboardService.Interfaces;

public interface IDashboardService
{
    Task<DashboardStats> GetDashboardAsync();
}
