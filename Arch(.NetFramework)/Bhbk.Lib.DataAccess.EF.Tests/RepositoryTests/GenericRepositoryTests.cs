using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataState.Expressions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Xunit;
using FakeConstants = Bhbk.Lib.DataAccess.EF.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    public class GenericRepositoryTests : BaseRepositoryTests
    {
        [Fact]
        public void Repo_Generic_Create_Fail_Entities()
        {
            Assert.Throws<DbUpdateException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                UoW.Users.Create(
                    new List<Users>() {
                    new Users() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() },
                    new Users() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() },
                    });
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Create_Fail_Entity()
        {
            Assert.Throws<DbUpdateException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                UoW.Users.Create(new Users() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() });
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Create_Success_Entities()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var location = (UoW.Locations.Get()).First();
            var users = new List<Users>();

            for (int i = 0; i < 3; i++)
                users.Add(new Users()
                {
                    userID = Guid.NewGuid(),
                    locationID = location.locationID,
                    int1 = FakeConstants.TestInteger,
                    date1 = DateTime.Now,
                    decimal1 = FakeConstants.TestDecimal,
                });

            UoW.Users.Create(users);
            UoW.Commit();
        }

        [Fact]
        public void Repo_Generic_Create_Success_Entity()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var location = (UoW.Locations.Get()).First();

            UoW.Users.Create(
                new Users()
                {
                    userID = Guid.NewGuid(),
                    locationID = location.locationID,
                    int1 = FakeConstants.TestInteger,
                    date1 = DateTime.Now,
                    decimal1 = FakeConstants.TestDecimal,
                });
            UoW.Commit();
        }

        [Fact]
        public void Repo_Generic_Delete_Fail_Entities()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                UoW.Users.Delete(
                    new List<Users>() {
                    new Users() { userID = Guid.NewGuid() },
                    new Users() { userID = Guid.NewGuid() },
                    });
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Delete_Fail_Entity()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                UoW.Users.Delete(new Users() { userID = Guid.NewGuid() });
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Delete_Fail_Lambda()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                var wrongExpr = new QueryExpression<Roles>().Where(x => x.roleID != Guid.NewGuid()).ToLambda();

                UoW.Users.Delete(wrongExpr);
                UoW.Commit();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                var wrongExpr = new QueryExpression<Roles>().Where(x => x.Users.Any(y => y.userID != Guid.NewGuid())).ToLambda();

                UoW.Users.Delete(wrongExpr);
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Delete_Success_Entities()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var user = (UoW.Users.Get()).First();

            UoW.Users.Delete(new List<Users>() { user });
            UoW.Commit();
        }

        [Fact]
        public void Repo_Generic_Delete_Success_Entity()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var user = (UoW.Users.Get()).First();

            UoW.Users.Delete(user);
            UoW.Commit();
        }

        [Fact]
        public void Repo_Generic_Delete_Success_Lambda()
        {
            //try entity...
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var userExpr = new QueryExpression<Users>().Where(x => x.userID != Guid.NewGuid()).ToLambda();

            UoW.Users.Delete(userExpr);
            UoW.Commit();

            //try entity navigation...
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            userExpr = new QueryExpression<Users>().Where(x => x.Roles.Any(y => y.roleID != Guid.NewGuid())).ToLambda();

            UoW.Users.Delete(userExpr);
            UoW.Commit();
        }

        [Fact]
        public void Repo_Generic_Get_Fail_Lambda()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var wrongExpr = new QueryExpression<Roles>().Where(x => x.roleID != Guid.NewGuid()).ToLambda();

            Assert.Throws<ArgumentException>(() =>
            {
                var users = UoW.Users.Get(wrongExpr);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                var users = UoW.Users.GetAsNoTracking(wrongExpr);
            });
        }

        [Fact]
        public void Repo_Generic_Get_Success_Lambda()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var userExpr = new QueryExpression<Users>().Where(x => x.userID != Guid.NewGuid()).ToLambda();

            var users = UoW.Users.Get(userExpr);
            users = UoW.Users.GetAsNoTracking(userExpr);
        }

        [Fact]
        public void Repo_Generic_Get_Success_NoParam()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var users = UoW.Users.Get();
            users.Count().Should().Be(10);

            users = UoW.Users.GetAsNoTracking();
            users.Count().Should().Be(10);
        }

        [Fact]
        public void Repo_Generic_Update_Fail_Entities()
        {
            Assert.Throws<DbUpdateConcurrencyException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                UoW.Users.Update(
                    new List<Users>() {
                    new Users() { userID = Guid.NewGuid() },
                    new Users() { userID = Guid.NewGuid() },
                    });
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Update_Fail_Entity()
        {
            Assert.Throws<DbUpdateConcurrencyException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                UoW.Users.Update(new Users() { userID = Guid.NewGuid() });
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Update_Success_Entities()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var locations = UoW.Locations.Get();
            var location1 = locations.First();
            var location2 = locations.Last();

            var users = UoW.Users.Get();
            var user1 = users.First();
            var user2 = users.Last();

            user1.locationID = location2.locationID;
            user1.description = FakeConstants.TestDesc;

            user2.locationID = location1.locationID;
            user2.description = FakeConstants.TestDesc;

            UoW.Users.Update(new List<Users> { user1, user2 });
            UoW.Commit();
        }

        [Fact]
        public void Repo_Generic_Update_Success_Entity()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var location = (UoW.Locations.Get()).First();
            var user = (UoW.Users.Get()).First();

            user.locationID = location.locationID;
            user.description = FakeConstants.TestDesc;

            UoW.Users.Update(user);
            UoW.Commit();
        }
    }
}
