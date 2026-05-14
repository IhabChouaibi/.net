namespace DashboardService.Models;

public class DashboardStats
{
    public int TotalClients { get; set; }
    public int TotalLivreurs { get; set; }
    public int TotalColis { get; set; }
    public decimal TotalRevenue { get; set; }
    public int DeliveredPackages { get; set; }
    public int PendingPackages { get; set; }
    public IList<ChartPoint> PackageStatusChart { get; set; } = new List<ChartPoint>();
    public IList<ChartPoint> PaymentByMonthChart { get; set; } = new List<ChartPoint>();
    public IList<ActivityItem> RecentActivities { get; set; } = new List<ActivityItem>();
    public IList<RecentDelivery> LatestDeliveries { get; set; } = new List<RecentDelivery>();
    public IList<RecentClient> LatestClients { get; set; } = new List<RecentClient>();
}

public class ChartPoint
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

public class ActivityItem
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Info";
}

public class RecentDelivery
{
    public string TrackingNumber { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
}

public class RecentClient
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
