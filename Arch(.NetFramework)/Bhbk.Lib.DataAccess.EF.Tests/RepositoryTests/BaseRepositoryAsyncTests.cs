using Bhbk.Lib.DataAccess.EF.Tests.UnitOfWork;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    public class BaseRepositoryAsyncTests
    {
        protected ISampleUoWAsync UoW;

        public BaseRepositoryAsyncTests()
        {
            UoW = new SampleUoWAsync();
        }
    }
}
