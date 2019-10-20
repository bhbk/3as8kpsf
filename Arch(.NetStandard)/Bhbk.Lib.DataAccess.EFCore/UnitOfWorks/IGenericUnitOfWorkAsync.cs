using Bhbk.Lib.Common.Primitives.Enums;
using System;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EFCore.UnitOfWorks
{
    public interface IGenericUnitOfWorkAsync : IAsyncDisposable
    {
        InstanceContext InstanceType { get; }
        ValueTask CommitAsync();
    }
}
