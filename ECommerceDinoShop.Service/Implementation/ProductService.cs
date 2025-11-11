using AutoMapper;
using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Model;
using ECommerceDinoShop.Repository.Contract;
using ECommerceDinoShop.Service.Contract;
using Microsoft.EntityFrameworkCore;

namespace ECommerceDinoShop.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _modelRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDTO>> Catalog(string category, string search)
        {
            try
            {
                var consult = _modelRepository.Consult(p =>
                p.Name.ToLower().Contains(search.ToLower()) &&
                p.IdCategoryNavigation.Name.ToLower().Contains(category.ToLower())
                );

                List<ProductDTO> list = _mapper.Map<List<ProductDTO>>(await consult.ToListAsync());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductDTO> Create(ProductDTO model)
        {
            try
            {
                var dbModel = _mapper.Map<Product>(model);
                var rspModel = await _modelRepository.Create(dbModel);

                if (rspModel.IdProduct != 0)
                    return _mapper.Map<ProductDTO>(rspModel);
                else
                    throw new TaskCanceledException("No se puede crear");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ;
        }

        public Task<List<ProductDTO>> List(string search)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDTO> Obtain(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ProductDTO model)
        {
            throw new NotImplementedException();
        }
    }
