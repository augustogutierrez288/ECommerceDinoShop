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

        public async Task<List<ProductDTO>> List(string search)
        {
            try
            {
                var consult = _modelRepository.Consult(p =>
                p.Name.ToLower().Contains(search.ToLower())
                );

                consult = consult.Include(c => c.IdCategoryNavigation);

                List<ProductDTO> list = _mapper.Map<List<ProductDTO>>(await consult.ToListAsync());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductDTO> Obtain(int id)
        {
            try
            {
                var consult = _modelRepository.Consult(p => p.IdProduct == id);
                consult = consult.Include(c => c.IdCategoryNavigation);
                var fromDbModel = await consult.FirstOrDefaultAsync();

                if (fromDbModel != null)
                    return _mapper.Map<ProductDTO>(fromDbModel);
                else
                    throw new TaskCanceledException("No se encontraron resultados");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var consulta = _modelRepository.Consult(p => p.IdProduct == id);
                var fromDbModel = await consulta.FirstOrDefaultAsync();

                if (fromDbModel != null)
                {
                    var response = await _modelRepository.Delete(fromDbModel);

                    if (!response)
                        throw new TaskCanceledException("No se pudo eliminar");

                    return response;
                }
                else
                {
                    throw new TaskCanceledException("No se encontraron resultados");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Update(ProductDTO model)
        {
            try
            {
                var consult = _modelRepository.Consult(p => p.IdProduct == model.IdProduct);
                var fromDbModel = await consult.FirstOrDefaultAsync();

                if (fromDbModel != null)
                {
                    fromDbModel.Name = model.Name;
                    fromDbModel.Description = model.Description;
                    fromDbModel.IdCategory = model.IdCategory;
                    fromDbModel.Quantity = model.Quantity;
                    fromDbModel.Price = model.Price;
                    fromDbModel.SalePrice = model.SalePrice;
                    fromDbModel.ImageUrl = model.ImageUrl;


                    var response = await _modelRepository.Update(fromDbModel);

                    if (!response)
                        throw new TaskCanceledException("No se pudo editar");

                    return response;
                }
                else
                {
                    throw new TaskCanceledException("No se encontraron resultados");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}