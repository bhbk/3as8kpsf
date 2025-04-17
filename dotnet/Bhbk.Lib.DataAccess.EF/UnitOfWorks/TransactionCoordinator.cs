using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Bhbk.Lib.DataAccess.EF.UnitOfWorks
{
    public class TransactionCoordinator : ITransactionCoordinator
    {
        private readonly TransactionScopeOption _scopeOption;
        private readonly TransactionOptions _transactionOptions;

        public TransactionCoordinator()
            : this(TransactionScopeOption.Required, IsolationLevel.ReadCommitted, TimeSpan.FromMinutes(1))
        {
        }

        public TransactionCoordinator(
            TransactionScopeOption scopeOption,
            IsolationLevel isolationLevel,
            TimeSpan timeout)
        {
            _scopeOption = scopeOption;
            _transactionOptions = new TransactionOptions
            {
                IsolationLevel = isolationLevel,
                Timeout = timeout
            };
        }

        public void ExecuteInTransaction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (var scope = new TransactionScope(_scopeOption, _transactionOptions))
            {
                action();
                scope.Complete();
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken ct = default)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            ct.ThrowIfCancellationRequested();

            using (var scope = new TransactionScope(
                _scopeOption,
                _transactionOptions,
                TransactionScopeAsyncFlowOption.Enabled))
            {
                await action();
                scope.Complete();
            }
        }
    }
}
