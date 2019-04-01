using System;
using System.Threading.Tasks;

namespace Bhbk.Lib.Core.UnitOfWork
{
    //https://en.wikipedia.org/wiki/Dependency_inversion_principle
    public interface IGenericUnitOfWorkAsync : IDisposable
    {
        ExecutionType Situation { get; }
        Task CommitAsync();
    }
}
