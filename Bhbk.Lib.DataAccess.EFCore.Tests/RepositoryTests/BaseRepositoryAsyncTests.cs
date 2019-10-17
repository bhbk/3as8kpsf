using Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWork;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Bhbk.Lib.DataAccess.EFCore.Tests.RepositoryTests
{
    [CollectionDefinition("RepositoryTests")]
    public class StartupAsyncTestsCollection : ICollectionFixture<BaseRepositoryAsyncTests> { }

    public class BaseRepositoryAsyncTests
    {
        protected ISampleUoWAsync UoW;

        public BaseRepositoryAsyncTests()
        {
            UoW = new SampleUoWAsync();
        }
    }
}
