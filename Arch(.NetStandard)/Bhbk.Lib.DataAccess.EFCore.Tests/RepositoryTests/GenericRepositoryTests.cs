using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Bhbk.Lib.DataState.Expressions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.RepositoryTests
{
    [Collection("RepositoryTests")]
    public class GenericRepositoryTests : BaseRepositoryTests
    {
        [Fact(Skip = "NotImplemented")]
        public void Repo_Generic_GetV1_Fail()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [Fact]
        public void Repo_Generic_GetV1_Success()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

            //var first = new QueryExpression<Users>().First().ToLambda();
            //UoW.UserRepo.Get(first);

            var where = new QueryExpression<Users>().Where(x => x.date1 < DateTime.Now).ToLambda();
            UoW.UserRepo.Get(where);
        }

        [Fact(Skip = "NotImplemented")]
        public void Repo_Generic_CreateV1_Fail()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [Fact(Skip = "NotImplemented")]
        public void Repo_Generic_CreateV1_Success()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [Fact(Skip = "NotImplemented")]
        public void Repo_Generic_UpdateV1_Fail()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [Fact(Skip = "NotImplemented")]
        public void Repo_Generic_UpdateV1_Success()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }

        [Fact]
        public void Repo_Generic_DeleteV1_Fail()
        {
            Assert.Throws<DbUpdateConcurrencyException>(() =>
            {
                UoW.UserRepo.Delete(new Users());
                UoW.Commit();

                UoW.RoleRepo.Delete(new Roles());
                UoW.Commit();

                UoW.LocationRepo.Delete(new Locations());
                UoW.Commit();
            });
        }

        [Fact(Skip = "NotImplemented")]
        public void Repo_Generic_DeleteV1_Success()
        {
            UoW.DataDestroy();
            UoW.DataCreate(10);

        }
    }
}
