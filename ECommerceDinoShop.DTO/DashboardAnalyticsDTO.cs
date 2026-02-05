namespace ECommerceDinoShop.DTO
{
    public class DashboardAnalyticsDTO
    {
        // Top Clients (Labels: Name, Values: Total payments)
        public List<string> TopClientsLabels { get; set; } = new();
        public List<decimal> TopClientsValues { get; set; } = new();

        // Ventas Mensuales (Labels: Mes, Values: Total)
        public List<string> MonthlyRevenueLabels { get; set; } = new();
        public List<decimal> MonthlyRevenueValues { get; set; } = new();

        // Top 5 Products (Labels: Name, Values: Cantidad Vendida)
        public List<string> TopProductsLabels { get; set; } = new();
        public List<int> TopProductsValues { get; set; } = new();

        // Ventas últimos 7 días
        public List<string> DailySalesLabels { get; set; } = new();
        public List<decimal> DailySalesValues { get; set; } = new();
    }
}
