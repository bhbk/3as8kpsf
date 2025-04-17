using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.UnitOfWorks
{
    public interface ITransactionCoordinator
    {
        void ExecuteInTransaction(Action action);
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken ct = default);
    }
}
