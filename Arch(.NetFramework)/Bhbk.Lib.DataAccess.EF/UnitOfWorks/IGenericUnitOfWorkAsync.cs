using Bhbk.Lib.Common.Primitives.Enums;
using System;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.UnitOfWorks
{
    public interface IGenericUnitOfWorkAsync : IDisposable
    {
        InstanceContext InstanceType { get; }
        Task CommitAsync();
    }
}
