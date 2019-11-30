using Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    [CollectionDefinition("RepositoryTests")]
    public class BaseRepositoryTestsCollection : ICollectionFixture<BaseRepositoryTests> { }

    public class BaseRepositoryTests
    {
        protected ISampleUoW UoW;

        public BaseRepositoryTests()
        {
            UoW = new SampleUoW();
        }
    }
}
