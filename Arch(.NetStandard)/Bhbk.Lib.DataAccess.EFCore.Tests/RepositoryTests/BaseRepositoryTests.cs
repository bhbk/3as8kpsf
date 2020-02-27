using Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.RepositoryTests
{
    [CollectionDefinition("RepositoryTests")]
    public class BaseRepositoryTestsCollection : ICollectionFixture<BaseRepositoryTests> { }

    public class BaseRepositoryTests
    {
        protected IUnitOfWork UoW;

        public BaseRepositoryTests()
        {
            UoW = new UnitOfWork();
        }
    }
}
