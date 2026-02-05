using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Model;
using ECommerceDinoShop.Repository.Contract;
using Microsoft.EntityFrameworkCore;

namespace ECommerceDinoShop.Repository.Implementation
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DbdinoShopContext _dbContext;

        public OrderRepository(DbdinoShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetAllOrdersWithDetails()
        {
            return await _dbContext.Orders
                .Include(u => u.IdUserNavigation)
                .Include(od => od.OrderDetails)
                    .ThenInclude(p => p.IdProductNavigation)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<DashboardAnalyticsDTO> GetAnalyticsAsync()
        {
            var analytics = new DashboardAnalyticsDTO();
            var now = DateTime.Now;

            // 1. Top 5 Clientes por Facturación
            var topClients = await _dbContext.Orders
                .Include(u => u.IdUserNavigation)
                .GroupBy(o => o.IdUserNavigation.FullName)
                .Select(g => new { Name = g.Key, Total = g.Sum(x => x.Total) })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .ToListAsync();

            analytics.TopClientsLabels = topClients.Select(x => x.Name).ToList();
            analytics.TopClientsValues = topClients.Select(x => x.Total ?? 0).ToList();

            // 2. Facturación Mensual (Año actual)
            var monthly = await _dbContext.Orders
                .Where(o => o.CreatedAt.Value.Year == now.Year)
                .GroupBy(o => o.CreatedAt.Value.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Total) })
                .OrderBy(x => x.Month)
                .ToListAsync();

            // Rellenar meses vacíos lógica rápida
            for (int i = 1; i <= 12; i++)
            {
                var mesData = monthly.FirstOrDefault(m => m.Month == i);
                analytics.MonthlyRevenueLabels.Add(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i));
                analytics.MonthlyRevenueValues.Add(mesData?.Total ?? 0);
            }

            // 3. Top 5 Productos más vendidos
            var topProducts = await _dbContext.OrderDetails
                .Include(p => p.IdProductNavigation)
                .GroupBy(d => d.IdProductNavigation.Name)
                .Select(g => new { Name = g.Key, Quantity = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToListAsync();

            analytics.TopProductsLabels = topProducts.Select(x => x.Name).ToList();
            analytics.TopProductsValues = topProducts.Select(x => x.Quantity ?? 0).ToList();

            // 4. Ventas últimos 7 días
            var last7Days = await _dbContext.Orders
                .Where(o => o.CreatedAt >= now.AddDays(-7))
                .GroupBy(o => o.CreatedAt.Value.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Total) })
                .OrderBy(x => x.Date)
                .ToListAsync();

            foreach (var item in last7Days)
            {
                analytics.DailySalesLabels.Add(item.Date.ToString("dd/MM"));
                analytics.DailySalesValues.Add(item.Total ?? 0);
            }

            return analytics;
        }

        public async Task<Order> Register(Order model)
        {
            Order orderGenerated = new Order();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (OrderDetail od in model.OrderDetails)
                    {
                        Product productoFound = _dbContext.Products.Where(p => p.IdProduct == od.IdProduct).First();
                        productoFound.Quantity = productoFound.Quantity - od.Quantity;
                        _dbContext.Products.Update(productoFound);
                    }

                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Orders.AddAsync(model);
                    await _dbContext.SaveChangesAsync();
                    orderGenerated = model;
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

                return orderGenerated;
            }
        }
    }
}
