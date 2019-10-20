using Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.RepositoryTests
{
    [CollectionDefinition("RepositoryTests")]
    public class StartupTestsCollection : ICollectionFixture<BaseRepositoryTests> { }

    public class BaseRepositoryTests
    {
        protected ISampleUoW UoW;

        public BaseRepositoryTests()
        {
            UoW = new SampleUoW();
        }
    }
}
