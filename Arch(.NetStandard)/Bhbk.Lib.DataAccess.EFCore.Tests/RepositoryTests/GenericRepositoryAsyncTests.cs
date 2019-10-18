using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Bhbk.Lib.DataState.Expressions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.RepositoryTests
{
    [Collection("RepositoryAsyncTests")]
    public class GenericRepositoryAsyncTests : BaseRepositoryAsyncTests
    {
        [Fact(Skip = "NotImplemented")]
        public async Task Repo_GenericAsync_GetV1_Fail()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);

        }

        [Fact]
        public async Task Repo_GenericAsync_GetV1_Success()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);

            //var first = new QueryExpression<Users>().First().ToLambda();
            //await UoW.UserRepo.GetAsync(first);

            var where = new QueryExpression<Users>().Where(x => x.date1 < DateTime.Now).ToLambda();
            await UoW.UserRepo.GetAsync(where);
        }

        [Fact(Skip = "NotImplemented")]
        public async Task Repo_GenericAsync_CreateV1_Fail()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);

        }

        [Fact(Skip = "NotImplemented")]
        public async Task Repo_GenericAsync_CreateV1_Success()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);

        }

        [Fact(Skip = "NotImplemented")]
        public async Task Repo_GenericAsync_UpdateV1_Fail()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);

        }

        [Fact(Skip = "NotImplemented")]
        public async Task Repo_GenericAsync_UpdateV1_Success()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);

        }

        [Fact]
        public async Task Repo_GenericAsync_DeleteV1_Fail()
        {
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await UoW.UserRepo.DeleteAsync(new Users());
                await UoW.CommitAsync();

                await UoW.RoleRepo.DeleteAsync(new Roles());
                await UoW.CommitAsync();

                await UoW.LocationRepo.DeleteAsync(new Locations());
                await UoW.CommitAsync();
            });
        }

        [Fact(Skip = "NotImplemented")]
        public async Task Repo_GenericAsync_DeleteV1_Success()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);

        }
    }
}
