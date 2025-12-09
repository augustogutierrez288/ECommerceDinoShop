using AutoMapper;
using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Model;
using ECommerceDinoShop.Repository.Contract;
using ECommerceDinoShop.Service.Contract;
using Microsoft.EntityFrameworkCore;

namespace ECommerceDinoShop.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _modelRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<SesionDTO> Authorization(LoginDTO model)
        {
            try
            {
                var consult = _modelRepository.Consult(p => p.Email == model.Email && p.Password == model.Password);
                var fromDbModel = await consult.FirstOrDefaultAsync();

                if (fromDbModel != null)
                    return _mapper.Map<SesionDTO>(fromDbModel);
                else
                    throw new TaskCanceledException("No se encontro coincidencias");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDTO> Create(UserDTO model)
        {
            try
            {
                var dbModel = _mapper.Map<User>(model);
                var rspModel = await _modelRepository.Create(dbModel);

                if (rspModel.IdUser != 0)
                    return _mapper.Map<UserDTO>(rspModel);
                else
                    throw new TaskCanceledException("No se puede crear");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserDTO>> List(string role, string search)
        {
            try
            {
                var consult = _modelRepository.Consult(p =>
                p.Role == role &&
                string.Concat(p.FullName.ToLower(), p.Email.ToLower()).Contains(search.ToLower())
                );

                List<UserDTO> list = _mapper.Map < List <UserDTO>>(await consult.ToListAsync());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDTO> Obtain(int id)
        {
            try
            {
                var consult = _modelRepository.Consult(p => p.IdUser == id);
                var fromDbModel = await consult.FirstOrDefaultAsync();

                if (fromDbModel != null)
                    return _mapper.Map<UserDTO>(fromDbModel);
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
                var consult = _modelRepository.Consult(p => p.IdUser == id);
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

        public async Task<bool> Update(UserDTO model)
        {
            try
            {
                var consult = _modelRepository.Consult(p => p.IdUser == model.IdUser);
                var fromDbModel = await consult.FirstOrDefaultAsync();

                if (fromDbModel != null)
                {
                    fromDbModel.FullName = model.FullName;
                    fromDbModel.Email = model.Email;
                    fromDbModel.Password = model.Password;
                    fromDbModel.Role = model.Role;

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
