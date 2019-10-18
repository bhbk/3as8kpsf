using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataState.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    [TestClass]
    public class GenericRepositoryTests : BaseRepositoryTests
    {
        [TestMethod, Ignore]
        public void Repo_Generic_GetV1_Fail()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [TestMethod]
        public void Repo_Generic_GetV1_Success()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

            //var first = new QueryExpression<Users>().First().ToLambda();
            //UoW.UserRepo.Get(first);

            var where = new QueryExpression<Users>().Where(x => x.date1 < DateTime.Now).ToLambda();
            UoW.UserRepo.Get(where);
        }

        [TestMethod, Ignore]
        public void Repo_Generic_CreateV1_Fail()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [TestMethod, Ignore]
        public void Repo_Generic_CreateV1_Success()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [TestMethod, Ignore]
        public void Repo_Generic_UpdateV1_Fail()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [TestMethod, Ignore]
        public void Repo_Generic_UpdateV1_Success()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Repo_Generic_DeleteV1_Fail()
        {
            UoW.UserRepo.Delete(new Users());
            UoW.Commit();

            UoW.RoleRepo.Delete(new Roles());
            UoW.Commit();

            UoW.LocationRepo.Delete(new Locations());
            UoW.Commit();
        }

        [TestMethod, Ignore]
        public void Repo_Generic_DeleteV1_Success()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }
    }
}
