using AutoMapper;
using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Model;
using ECommerceDinoShop.Repository.Contract;
using ECommerceDinoShop.Service.Contract;
using Microsoft.EntityFrameworkCore;

namespace ECommerceDinoShop.Service.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _modelRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper; 
        }

        public async Task<CategoryDTO> Create(CategoryDTO model)
        {
            try
            {
                var dbModel = _mapper.Map<Category>(model);
                var rspModel = await _modelRepository.Create(dbModel);

                if (rspModel.IdCategory != 0)
                    return _mapper.Map<CategoryDTO>(rspModel);
                else
                    throw new TaskCanceledException("No se puede crear");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CategoryDTO>> List(string search)
        {
            try
            {
                var consult = _modelRepository.Consult(p =>
                p.Name!.ToLower().Contains(search.ToLower())
                );

                List<CategoryDTO> list = _mapper.Map<List<CategoryDTO>>(await consult.ToListAsync());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryDTO> Obtain(int id)
        {
            try
            {
                var consult = _modelRepository.Consult(p => p.IdCategory == id);
                var fromDbModel = await consult.FirstOrDefaultAsync();

                if (fromDbModel != null)
                    return _mapper.Map<CategoryDTO>(fromDbModel);
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
                var consult = _modelRepository.Consult(p => p.IdCategory == id);
                var fromDbModel = await consult.FirstOrDefaultAsync();

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

        public async Task<bool> Update(CategoryDTO model)
        {
            try
            {
                var consult = _modelRepository.Consult(p => p.IdCategory == model.IdCategory);
                var fromDbModel = await consult.FirstOrDefaultAsync();

                if (fromDbModel != null)
                {
                    fromDbModel.Name = model.Name;

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
