namespace LivraisonFrontend.ViewModels;

public class DashboardViewModel
{
    public int TotalClients { get; set; }

    public int TotalLivreurs { get; set; }

    public int TotalColis { get; set; }

    public decimal TotalRevenue { get; set; }

    public int DeliveredPackages { get; set; }

    public int PendingPackages { get; set; }

    public IList<ChartPointViewModel> PackageStatusChart { get; set; } = new List<ChartPointViewModel>();

    public IList<ChartPointViewModel> PaymentByMonthChart { get; set; } = new List<ChartPointViewModel>();

    public IList<ActivityItemViewModel> RecentActivities { get; set; } = new List<ActivityItemViewModel>();

    public IList<RecentDeliveryViewModel> LatestDeliveries { get; set; } = new List<RecentDeliveryViewModel>();

    public IList<RecentClientViewModel> LatestClients { get; set; } = new List<RecentClientViewModel>();
}

public class ChartPointViewModel
{
    public string Label { get; set; } = string.Empty;

    public decimal Value { get; set; }
}

public class ActivityItemViewModel
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Info";
}

public class RecentDeliveryViewModel
{
    public string TrackingNumber { get; set; } = string.Empty;

    public string DriverName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime ScheduledDate { get; set; } = DateTime.UtcNow;
}

public class RecentClientViewModel
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
