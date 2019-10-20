using Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Bhbk.Lib.DataAccess.EFCore.Tests.RepositoryTests
{
    [CollectionDefinition("RepositoryAsyncTests")]
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
