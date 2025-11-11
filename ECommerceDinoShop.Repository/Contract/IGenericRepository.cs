using System.Linq.Expressions;

namespace ECommerceDinoShop.Repository.Contract
{
    public interface IGenericRepository<TModel> where TModel : class //Aqui digo que la interfaz recibe una entidad de tipo clase
    {
        IQueryable<TModel> Consult(Expression<Func<TModel, bool>>? filter = null);
        Task<TModel> Create(TModel model);
        Task<bool> Update(TModel model);
        Task<bool> Delete(TModel model);
    }
}
