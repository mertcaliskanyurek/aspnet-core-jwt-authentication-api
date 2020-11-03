using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BaseApi.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T obj);
        Task<long> CountWhereAsync(Expression<Func<T, bool>> expression);
        void Update(T obj);
        void Remove(T obj);
        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

    }
}
