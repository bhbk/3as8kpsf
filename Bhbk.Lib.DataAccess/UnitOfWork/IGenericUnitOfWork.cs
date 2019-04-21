using Bhbk.Lib.Common.Primitives.Enums;
using System;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.UnitOfWork
{
    //https://en.wikipedia.org/wiki/Dependency_inversion_principle
    public interface IGenericUnitOfWork : IDisposable
    {
        InstanceContext InstanceType { get; }
        Task Commit();
    }
}
