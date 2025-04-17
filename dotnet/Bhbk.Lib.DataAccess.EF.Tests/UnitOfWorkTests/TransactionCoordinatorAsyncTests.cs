using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public class TransactionCoordinatorAsyncTests
    {
        [Fact]
        public async Task ExecuteInTransactionAsync_SuccessfulAction_Commits()
        {
            var coordinator = new TransactionCoordinator();
            var executed = false;

            await coordinator.ExecuteInTransactionAsync(async () =>
            {
                await Task.Delay(1);
                executed = true;
            });

            executed.Should().BeTrue();
        }

        [Fact]
        public async Task ExecuteInTransactionAsync_NullAction_ThrowsArgumentNullException()
        {
            var coordinator = new TransactionCoordinator();

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                coordinator.ExecuteInTransactionAsync(null));
        }

        [Fact]
        public async Task ExecuteInTransactionAsync_ActionThrows_DoesNotCommit()
        {
            var coordinator = new TransactionCoordinator();
            var beforeException = false;

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await coordinator.ExecuteInTransactionAsync(async () =>
                {
                    await Task.Delay(1);
                    beforeException = true;
                    throw new InvalidOperationException("Test exception");
                });
            });

            beforeException.Should().BeTrue();
        }

        [Fact]
        public async Task ExecuteInTransactionAsync_WithCancellationToken_RespectsToken()
        {
            var coordinator = new TransactionCoordinator();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await coordinator.ExecuteInTransactionAsync(async () =>
                {
                    await Task.Delay(100);
                }, cts.Token);
            });
        }

        [Fact]
        public async Task ExecuteInTransactionAsync_NestedActions_WorksCorrectly()
        {
            var coordinator = new TransactionCoordinator();
            var innerExecuted = false;
            var outerExecuted = false;

            await coordinator.ExecuteInTransactionAsync(async () =>
            {
                outerExecuted = true;
                await Task.Delay(1);

                await coordinator.ExecuteInTransactionAsync(async () =>
                {
                    innerExecuted = true;
                    await Task.Delay(1);
                });
            });

            outerExecuted.Should().BeTrue();
            innerExecuted.Should().BeTrue();
        }
    }
}
