using Bhbk.Lib.Core.Primitives.Enums;
using System;
using System.Threading.Tasks;

namespace Bhbk.Lib.Core.Interfaces
{
    //https://en.wikipedia.org/wiki/Dependency_inversion_principle
    public interface IGenericUnitOfWorkAsync : IDisposable
    {
        InstanceContext InstanceType { get; }
        Task CommitAsync();
    }
}
