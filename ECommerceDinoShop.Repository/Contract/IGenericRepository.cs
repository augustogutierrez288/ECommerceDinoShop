using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceDinoShop.Repository.Contract
{
    public interface IGenericRepository<TModel> where TModel : class //Aqui digo que la interfaz recibe una entidad de tipo clase
    {
        IQueryable<TModel> Consult(Expression<Func<TModel, bool>>? filter = null);
        Task<TModel> Create(TModel modelo);
        Task<bool> Update(TModel modelo);
        Task<bool> Delete(TModel modelo);
    }
}
