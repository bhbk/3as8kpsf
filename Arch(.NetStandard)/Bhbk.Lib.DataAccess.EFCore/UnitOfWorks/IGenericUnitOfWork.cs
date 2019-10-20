using Bhbk.Lib.Common.Primitives.Enums;
using System;

namespace Bhbk.Lib.DataAccess.EFCore.UnitOfWorks
{
    public interface IGenericUnitOfWork : IDisposable
    {
        InstanceContext InstanceType { get; }
        void Commit();
    }
}
