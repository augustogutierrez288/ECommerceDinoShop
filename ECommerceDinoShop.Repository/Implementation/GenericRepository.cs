using ECommerceDinoShop.Repository.Contract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceDinoShop.Repository.Implementation
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly DbdinoShopContext _dbContext;

        public GenericRepository(DbdinoShopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TModel> Consult(Expression<Func<TModel, bool>>? filter = null)
        {
            IQueryable<TModel> consult = (filter == null) ? _dbContext.Set<TModel>() : _dbContext.Set<TModel>().Where(filter);
            return consult;
        }

        public async Task<TModel> Create(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Add(model);
                await _dbContext.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Remove(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Update(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
