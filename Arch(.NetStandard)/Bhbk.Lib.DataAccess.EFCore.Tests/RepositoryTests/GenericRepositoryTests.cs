using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Bhbk.Lib.QueryExpression.Extensions;
using Bhbk.Lib.QueryExpression.Factories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FakeConstants = Bhbk.Lib.DataAccess.EFCore.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.RepositoryTests
{
    [Collection("RepositoryTests")]
    public class GenericRepositoryTests : BaseRepositoryTests
    {
        [Fact(Skip = "ShouldThrowException")]
        public void Repo_Generic_Create_Fail_Entities()
        {
            Assert.Throws<DbUpdateException>(() =>
            {
                UoW.DeleteDatasets();

                UoW.Users.Create(
                    new List<User>() {
                        new User() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() },
                        new User() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() },
                    });
                UoW.Commit();
            });
        }

        [Fact(Skip = "ShouldThrowException")]
        public void Repo_Generic_Create_Fail_Entity()
        {
            Assert.Throws<DbUpdateException>(() =>
            {
                UoW.DeleteDatasets();

                UoW.Users.Create(new User() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() });
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Create_Success_Entities()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var location = (UoW.Locations.Get()).First();
            var users = new List<User>();

            for (int i = 0; i < 3; i++)
                users.Add(new User()
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
                new User()
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
            Assert.Throws<DbUpdateConcurrencyException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                UoW.Users.Delete(
                    new List<User>() {
                        new User() { userID = Guid.NewGuid() },
                        new User() { userID = Guid.NewGuid() },
                    });
                UoW.Commit();
            });
        }

        [Fact]
        public void Repo_Generic_Delete_Fail_Entity()
        {
            Assert.Throws<DbUpdateConcurrencyException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                UoW.Users.Delete(new User() { userID = Guid.NewGuid() });
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

                var wrongExpr = QueryExpressionFactory.GetQueryExpression<Role>().Where(x => x.roleID != Guid.NewGuid()).ToLambda();

                UoW.Users.Delete(wrongExpr);
                UoW.Commit();
            });

            Assert.Throws<ArgumentException>(() =>
            {
                UoW.DeleteDatasets();
                UoW.CreateDatasets(10);

                var wrongExpr = QueryExpressionFactory.GetQueryExpression<Role>().Where(x => x.Members.Any(y => y.userID != Guid.NewGuid())).ToLambda();

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

            UoW.Users.Delete(new List<User>() { user });
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

            var userExpr = QueryExpressionFactory.GetQueryExpression<User>().Where(x => x.userID != Guid.NewGuid()).ToLambda();

            UoW.Users.Delete(userExpr);
            UoW.Commit();

            //try entity navigation...
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            userExpr = QueryExpressionFactory.GetQueryExpression<User>().Where(x => x.Members.Any(y => y.roleID != Guid.NewGuid())).ToLambda();

            UoW.Users.Delete(userExpr);
            UoW.Commit();
        }

        [Fact]
        public void Repo_Generic_Get_Fail_Lambda()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var wrongExpr = QueryExpressionFactory.GetQueryExpression<Role>().Where(x => x.roleID != Guid.NewGuid()).ToLambda();

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
        public void Repo_Generic_Get_Success_Function()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var users = UoW.Users.Get(x => x.userID != Guid.NewGuid());

            users = UoW.Users.Get(x => x.userID != Guid.NewGuid(),
                x => x.Include(y => y.Members));

            users = UoW.Users.Get(x => x.userID != Guid.NewGuid(),
                x => x.Include(y => y.Members),
                x => x.OrderBy(x => x.userID), 0, 1000);

            users = UoW.Users.GetAsNoTracking(x => x.userID != Guid.NewGuid());

            users = UoW.Users.GetAsNoTracking(x => x.userID != Guid.NewGuid(),
                x => x.Include(y => y.Members));

            users = UoW.Users.GetAsNoTracking(x => x.userID != Guid.NewGuid(),
                x => x.Include(y => y.Members),
                x => x.OrderBy(x => x.userID), 0, 1000);
        }

        [Fact]
        public void Repo_Generic_Get_Success_Lambda()
        {
            UoW.DeleteDatasets();
            UoW.CreateDatasets(10);

            var userExpr = QueryExpressionFactory.GetQueryExpression<User>().Where(x => x.userID != Guid.NewGuid()).ToLambda();

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
                    new List<User>() {
                        new User() { userID = Guid.NewGuid() },
                        new User() { userID = Guid.NewGuid() },
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

                UoW.Users.Update(new User() { userID = Guid.NewGuid() });
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

            UoW.Users.Update(new List<User> { user1, user2 });
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
