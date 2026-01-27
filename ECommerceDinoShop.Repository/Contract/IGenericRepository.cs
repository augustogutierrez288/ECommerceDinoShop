using System.Linq.Expressions;

namespace ECommerceDinoShop.Repository.Contract
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        IQueryable<TModel> Consult(Expression<Func<TModel, bool>>? filter = null);
        Task<TModel> Create(TModel model);
        Task<bool> Update(TModel model);
        Task<bool> Delete(TModel model);
    }
}
