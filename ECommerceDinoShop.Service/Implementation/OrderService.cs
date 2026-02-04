using AutoMapper;
using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Model;
using ECommerceDinoShop.Repository.Contract;
using ECommerceDinoShop.Repository.Implementation;
using ECommerceDinoShop.Service.Contract;
using Microsoft.EntityFrameworkCore;

namespace ECommerceDinoShop.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _modelRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDTO>> ListAsync()
        {
            try
            {
                var orders = await _modelRepository.GetAllOrdersWithDetails();
                return _mapper.Map<List<OrderDTO>>(orders);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<OrderDTO> Register(OrderDTO model)
        {
            try
            {
                var dbModel = _mapper.Map<Order>(model);
                var orderGenerated = await _modelRepository.Register(dbModel);

                if (orderGenerated.IdOrder == 0)
                    throw new TaskCanceledException("No se puede registrar venta");

                return _mapper.Map<OrderDTO>(orderGenerated);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
