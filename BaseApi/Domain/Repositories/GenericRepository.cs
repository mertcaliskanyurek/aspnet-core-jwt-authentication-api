using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BaseApi.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BaseApi.Domain.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        protected readonly DatabaseContext Context;

        private DbSet<T> _table = null;

        public GenericRepository(DatabaseContext context)
        {
            Context = context;
            _table = context.Set<T>();
        }

        /// <summary>
        /// Adds new entity object to database table.
        /// </summary>
        /// <param name="obj">A new object</param>
        public async Task AddAsync(T obj)
        {
            await _table.AddAsync(obj);
        }

        /// <summary>
        /// Updates given entity object based on related id.
        /// </summary>
        /// <param name="obj">New object data.</param>
        public void Update(T obj)
        {
            _table.Update(obj);
        }

        /// <summary>
        /// Removes related entity from database.
        /// </summary>
        /// <param name="user">Entity to remove.</param>
        public void Remove(T obj)
        {
            _table.Remove(obj);
        }

        /// <summary>
        /// Finds first entity list object with the given expression.
        /// </summary>
        /// <param name="expression">Delegate.</param>
        /// <returns>Found entity. Null otherwise.</returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.FirstOrDefaultAsync(expression);
        }

        /// <summary>
        /// Finds entity list object with the given expression.
        /// </summary>
        /// <param name="expression">Delegate.</param>
        /// <returns>The list of entities found. Empty list otherwise.</returns>
        public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.Where(expression).ToListAsync();
        }

        /// <summary>
        /// Counts result in count given expression.
        /// </summary>
        /// <param name="expression">Expression delegate.</param>
        /// <returns>Count of the result.</returns>
        public async Task<long> CountWhereAsync(Expression<Func<T, bool>> expression)
        {
            return await _table.CountAsync(expression);
        }

    }
}
