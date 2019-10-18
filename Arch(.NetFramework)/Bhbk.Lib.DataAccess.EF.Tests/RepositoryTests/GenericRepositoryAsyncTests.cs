using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataState.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    [TestClass]
    public class GenericRepositoryAsyncTests : BaseRepositoryAsyncTests
    {
        [TestMethod, Ignore]
        public async Task Repo_GenericAsync_GetV1_Fail()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);
        }

        [TestMethod]
        public void Repo_GenericAsync_ExploreV1_Success()
        {
            var sb = new StringBuilder();

            var methods = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "Where"
                    && x.GetParameters().Length == 2);

            foreach (var method in methods)
            {
                sb.Append("Method:" + method.Name + Environment.NewLine);

                var paramters = method.GetParameters();

                foreach (var parameter in paramters)
                {
                    sb.Append("  Parameter Name:" + parameter.Name + Environment.NewLine);
                    sb.Append("  Parameter Position:" + parameter.Position + Environment.NewLine);
                    sb.Append("  Parameter Type:" + parameter.ParameterType + Environment.NewLine);
                }
            }
        }

        [TestMethod]
        public async Task Repo_GenericAsync_GetV1_Success()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);

            //var first = new QueryExpression<Users>().First().ToLambda();
            //await UoW.UserRepo.GetAsync(first);

            var where = new QueryExpression<Users>().Where(x => x.date1 < DateTime.Now).ToLambda();
            await UoW.UserRepo.GetAsync(where);
        }

        [TestMethod, Ignore]
        public async Task Repo_GenericAsync_CreateV1_Fail()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);
        }

        [TestMethod, Ignore]
        public async Task Repo_GenericAsync_CreateV1_Success()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);
        }

        [TestMethod, Ignore]
        public async Task Repo_GenericAsync_UpdateV1_Fail()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);
        }

        [TestMethod, Ignore]
        public async Task Repo_GenericAsync_UpdateV1_Success()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Repo_GenericAsync_DeleteV1_Fail()
        {
            await UoW.UserRepo.DeleteAsync(new Users());
            await UoW.CommitAsync();

            await UoW.RoleRepo.DeleteAsync(new Roles());
            await UoW.CommitAsync();

            await UoW.LocationRepo.DeleteAsync(new Locations());
            await UoW.CommitAsync();
        }

        [TestMethod, Ignore]
        public async Task Repo_GenericAsync_DeleteV1_Success()
        {
            await UoW.DataDestroy();
            await UoW.DataCreate(10);
        }
    }
}
