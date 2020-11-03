using System;
using System.Threading.Tasks;

namespace BaseApi.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task ComplateAsync();
    }
}
