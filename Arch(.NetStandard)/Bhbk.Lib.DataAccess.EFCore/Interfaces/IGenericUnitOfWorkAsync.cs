using Bhbk.Lib.Common.Primitives.Enums;
using System;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EFCore.Interfaces
{
    public interface IGenericUnitOfWorkAsync : IDisposable
    {
        InstanceContext InstanceType { get; }
        Task CommitAsync();
    }
}
