using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BaseApi.Domain.Response;

namespace BaseApi.Domain.Services
{
    public interface IGenericService<T> where T:class
    {
        Task<ObjectResponse<T>> AddAsync(T obj);
        Task<ObjectResponse<T>> UpdateAsync(T obj);
        Task<ObjectResponse<T>> DeleteAsync(T obj);

        Task<ObjectResponse<T>> FindFirstOrDefault(Expression<Func<T, bool>> expression);
        Task<ObjectResponse<IEnumerable<T>>> WhereAsync(Expression<Func<T, bool>> expression);
        Task<long> CountWhereAsync(Expression<Func<T, bool>> expression);
    }
}
