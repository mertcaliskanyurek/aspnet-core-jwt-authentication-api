using System;
using System.Threading.Tasks;
using BaseApi.Domain.Entity;

namespace BaseApi.Domain.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private DatabaseContext _dbContext;

        public UnitOfWork(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Saves changes of the database context.
        /// </summary>
        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
