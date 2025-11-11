using ECommerceDinoShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceDinoShop.Repository.Contract
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> Register(Order model);
    }
}
