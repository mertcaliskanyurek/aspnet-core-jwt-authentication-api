using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BaseApi.Domain.Repositories;
using BaseApi.Domain.Response;
using BaseApi.Domain.Services;
using BaseApi.Domain.UnitOfWork;

namespace BaseApi.Services
{
    public class GenericService<T>:IGenericService<T> where T:class
    {
        protected readonly IGenericRepository<T> Repository;
        protected readonly IUnitOfWork UnitOfWork;

        public GenericService(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Adds new entity object to database.
        /// </summary>
        /// <param name="obj">Entity object.</param>
        /// <returns><see cref="ObjectResponse<T>"/> that includes <see cref="T"/> object
        /// if success. Error message otherwise.</returns>
        public virtual async Task<ObjectResponse<T>> AddAsync(T obj)
        {
            try
            {
                await Repository.AddAsync(obj);
                await UnitOfWork.CompleteAsync();
                return new ObjectResponse<T>(obj);
            }
            catch (Exception e)
            {
                return new ObjectResponse<T>($"Error while adding {typeof(T).Name} entity::{e.Message}");
            }

        }

        /// <summary>
        /// Removes entity if exist.
        /// </summary>
        /// <param name="obj">Entity to delete.</param>
        /// <returns></returns>
        public virtual async Task<ObjectResponse<T>> DeleteAsync(T obj)
        {
            try
            {
                Repository.Remove(obj);
                await UnitOfWork.CompleteAsync();
                return new ObjectResponse<T>(obj);
            }
            catch (Exception e)
            {
                return new ObjectResponse<T>($"Error while deleting {typeof(T).Name} entity::{e.Message}");
            }
        }

        /// <summary>
        /// Updates entity by related uniqe key.
        /// </summary>
        /// <param name="obj">Entity to update</param>
        /// <returns></returns>
        public virtual async Task<ObjectResponse<T>> UpdateAsync(T obj)
        {
            try
            {
                Repository.Update(obj);
                await UnitOfWork.CompleteAsync();
                return new ObjectResponse<T>(obj);
            }
            catch (Exception e)
            {
                return new ObjectResponse<T>($"Error while updating {typeof(T).Name} entity::{e.Message}");
            }
        }

        public async Task<ObjectResponse<T>> FindFirstOrDefault(Expression<Func<T, bool>> expression)
        {
            T obj = await Repository.FirstOrDefaultAsync(expression);
            if (obj == null) return new ObjectResponse<T>($"{typeof(T).Name} not found.");
            return new ObjectResponse<T>(obj);
        }

        /// <summary>
        /// Finds list of entity with the given expression.
        /// </summary>
        /// <param name="expression">Delegate</param>
        /// <returns>List of entity found, empty list otherwise.</returns>
        public async Task<ObjectResponse<IEnumerable<T>>> WhereAsync(Expression<Func<T, bool>> expression)
        {
            IEnumerable<T> entityList = await Repository.WhereAsync(expression);
            return new ObjectResponse<IEnumerable<T>>(entityList);
        }

        /// <summary>
        /// Counts entity with the given expression.
        /// </summary>
        /// <param name="expression">Delegate</param>
        /// <returns>Count of entity found, 0 otherwise.</returns>
        public async Task<long> CountWhereAsync(Expression<Func<T, bool>> expression)
        {
            return await Repository.CountWhereAsync(expression);
        }
    }
}
