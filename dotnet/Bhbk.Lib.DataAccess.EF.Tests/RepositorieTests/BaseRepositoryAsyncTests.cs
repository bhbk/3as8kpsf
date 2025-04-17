using Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    [CollectionDefinition("RepositoryAsyncTests")]
    public class BaseRepositoryAsyncTestsCollection : ICollectionFixture<BaseRepositoryAsyncTests> { }

    public class BaseRepositoryAsyncTests
    {
        protected IUnitOfWorkAsync UoW;

        public BaseRepositoryAsyncTests()
        {
            UoW = new UnitOfWorkAsync();
        }
    }
}
