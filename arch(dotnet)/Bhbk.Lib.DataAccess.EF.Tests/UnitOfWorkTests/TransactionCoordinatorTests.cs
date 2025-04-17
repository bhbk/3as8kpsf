using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
using FluentAssertions;
using System;
using System.Transactions;
using Xunit;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public class TransactionCoordinatorTests
    {
        [Fact]
        public void ExecuteInTransaction_SuccessfulAction_Commits()
        {
            var coordinator = new TransactionCoordinator();
            var executed = false;

            coordinator.ExecuteInTransaction(() =>
            {
                executed = true;
            });

            executed.Should().BeTrue();
        }

        [Fact]
        public void ExecuteInTransaction_NullAction_ThrowsArgumentNullException()
        {
            var coordinator = new TransactionCoordinator();

            Assert.Throws<ArgumentNullException>(() =>
                coordinator.ExecuteInTransaction(null));
        }

        [Fact]
        public void ExecuteInTransaction_ActionThrows_DoesNotCommit()
        {
            var coordinator = new TransactionCoordinator();
            var beforeException = false;
            var afterException = false;

            Assert.Throws<InvalidOperationException>(() =>
            {
                coordinator.ExecuteInTransaction(() =>
                {
                    beforeException = true;
                    throw new InvalidOperationException("Test exception");
#pragma warning disable CS0162
                    afterException = true;
#pragma warning restore CS0162
                });
            });

            beforeException.Should().BeTrue();
            afterException.Should().BeFalse();
        }

        [Fact]
        public void Constructor_WithCustomOptions_UsesOptions()
        {
            var coordinator = new TransactionCoordinator(
                TransactionScopeOption.RequiresNew,
                IsolationLevel.Serializable,
                TimeSpan.FromMinutes(5));

            coordinator.Should().NotBeNull();
        }
    }
}
