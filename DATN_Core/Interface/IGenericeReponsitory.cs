using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Interface
{
    public interface IGenericeReponsitory<T> where T : class

    {
        Task<IReadOnlyCollection<T>> GetAllAsync();
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, bool>>[] includes);
        IEnumerable<T> GetAll(params Expression<Func<T, bool>>[] includes);
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(T id);
        Task AddAsync(T entity);
        Task UpdateAsync(int id,T entity);
        Task DeleteAsync(int id);

    }
}
