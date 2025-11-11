using ECommerceDinoShop.Model;
using ECommerceDinoShop.Repository.Contract;

namespace ECommerceDinoShop.Repository.Implementation
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DbdinoShopContext _dbContext;

        public OrderRepository(DbdinoShopContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
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
