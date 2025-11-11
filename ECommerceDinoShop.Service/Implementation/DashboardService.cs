

using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Model;
using ECommerceDinoShop.Repository.Contract;
using ECommerceDinoShop.Service.Contract;

namespace ECommerceDinoShop.Service.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<User> _userRepository;

        public DashboardService(
            IOrderRepository orderRepository,
            IGenericRepository<Product> productRepository,
            IGenericRepository<User> userRepository
            )
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        private string Income() //Ingresos
        {
            var consult = _orderRepository.Consult();
            decimal? income = consult.Sum(x => x.Total);

            return Convert.ToString(income);
        }

        private int Order() //Ventas
        {
            var consult = _orderRepository.Consult();
            int total = consult.Count();

            return total;
        }

        private int Customers() //Clientes
        {
            var consult = _userRepository.Consult(u => u.Role.ToLower() == "cliente");
            int total = consult.Count();
            return total;
        }

        private int Products()
        {
            var consult = _productRepository.Consult();
            int total = consult.Count();
            return total;
        }

        public DashboardDTO Resume()
        {
            try
            {
                DashboardDTO dashboard = new DashboardDTO()
                {
                    TotalRevenue = Income(),
                    TotalOrders = Order(),
                    TotalCustomers = Customers(),
                    TotalProducts = Products(),

                };

                return dashboard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
