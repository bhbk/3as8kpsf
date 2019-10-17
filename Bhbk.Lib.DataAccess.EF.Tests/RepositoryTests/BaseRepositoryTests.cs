using Bhbk.Lib.DataAccess.EF.Tests.UnitOfWork;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    public class BaseRepositoryTests
    {
        protected ISampleUoW UoW;

        public BaseRepositoryTests()
        {
            UoW = new SampleUoW();
        }
    }
}
