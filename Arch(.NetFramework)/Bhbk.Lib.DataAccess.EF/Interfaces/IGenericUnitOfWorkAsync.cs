using Bhbk.Lib.Common.Primitives.Enums;
using System;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Interfaces
{
    public interface IGenericUnitOfWorkAsync : IDisposable
    {
        InstanceContext InstanceType { get; }
        Task CommitAsync();
    }
}
