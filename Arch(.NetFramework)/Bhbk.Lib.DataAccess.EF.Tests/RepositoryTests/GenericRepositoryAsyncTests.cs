﻿using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.QueryExpression;
using Bhbk.Lib.QueryExpression.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FakeConstants = Bhbk.Lib.DataAccess.EF.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    public class GenericRepositoryAsyncTests : BaseRepositoryAsyncTests
    {
        [Fact]
        public async Task Repo_GenericAsync_Create_Fail_Entities()
        {
            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await UoW.DeleteDatasets();

                await UoW.Users.CreateAsync(
                    new List<Users>() {
                        new Users() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() },
                        new Users() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() },
                    });
                await UoW.CommitAsync();
            });
        }

        [Fact]
        public async Task Repo_GenericAsync_Create_Fail_Entity()
        {
            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await UoW.DeleteDatasets();

                await UoW.Users.CreateAsync(new Users() { userID = Guid.NewGuid(), locationID = Guid.NewGuid() });
                await UoW.CommitAsync();
            });
        }

        [Fact]
        public async Task Repo_GenericAsync_Create_Success_Entities()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var location = (await UoW.Locations.GetAsync()).First();
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

            await UoW.Users.CreateAsync(users);
            await UoW.CommitAsync();
        }

        [Fact]
        public async Task Repo_GenericAsync_Create_Success_Entity()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var location = (await UoW.Locations.GetAsync()).First();

            await UoW.Users.CreateAsync(
                new Users()
                {
                    userID = Guid.NewGuid(),
                    locationID = location.locationID,
                    int1 = FakeConstants.TestInteger,
                    date1 = DateTime.Now,
                    decimal1 = FakeConstants.TestDecimal,
                });
            await UoW.CommitAsync();
        }

        [Fact]
        public async Task Repo_GenericAsync_Delete_Fail_Entities()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await UoW.DeleteDatasets();
                await UoW.CreateDatasets(10);

                await UoW.Users.DeleteAsync(
                    new List<Users>() {
                        new Users() { userID = Guid.NewGuid() },
                        new Users() { userID = Guid.NewGuid() },
                    });
                await UoW.CommitAsync();
            });
        }

        [Fact]
        public async Task Repo_GenericAsync_Delete_Fail_Entity()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await UoW.DeleteDatasets();
                await UoW.CreateDatasets(10);

                await UoW.Users.DeleteAsync(new Users() { userID = Guid.NewGuid() });
                await UoW.CommitAsync();
            });
        }

        [Fact]
        public async Task Repo_GenericAsync_Delete_Fail_Lambda()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await UoW.DeleteDatasets();
                await UoW.CreateDatasets(10);

                var wrongExpr = new QueryExpression<Roles>().Where(x => x.roleID != Guid.NewGuid()).ToLambda();

                await UoW.Users.DeleteAsync(wrongExpr);
                await UoW.CommitAsync();
            });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await UoW.DeleteDatasets();
                await UoW.CreateDatasets(10);

                var wrongExpr = new QueryExpression<Roles>().Where(x => x.Users.Any(y => y.userID != Guid.NewGuid())).ToLambda();

                await UoW.Users.DeleteAsync(wrongExpr);
                await UoW.CommitAsync();
            });
        }

        [Fact]
        public async Task Repo_GenericAsync_Delete_Success_Entities()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var user = (await UoW.Users.GetAsync()).First();

            await UoW.Users.DeleteAsync(new List<Users>() { user });
            await UoW.CommitAsync();
        }

        [Fact]
        public async Task Repo_GenericAsync_Delete_Success_Entity()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var user = (await UoW.Users.GetAsync()).First();

            await UoW.Users.DeleteAsync(user);
            await UoW.CommitAsync();
        }

        [Fact]
        public async Task Repo_GenericAsync_Delete_Success_Lambda()
        {
            //try entity...
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var userExpr = new QueryExpression<Users>().Where(x => x.userID != Guid.NewGuid()).ToLambda();

            await UoW.Users.DeleteAsync(userExpr);
            await UoW.CommitAsync();

            //try entity navigation...
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            userExpr = new QueryExpression<Users>().Where(x => x.Roles.Any(y => y.roleID != Guid.NewGuid())).ToLambda();

            await UoW.Users.DeleteAsync(userExpr);
            await UoW.CommitAsync();
        }

        [Fact]
        public async Task Repo_GenericAsync_Get_Fail_Lambda()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var wrongExpr = new QueryExpression<Roles>().Where(x => x.roleID != Guid.NewGuid()).ToLambda();

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var users = await UoW.Users.GetAsync(wrongExpr);
            });

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                var users = await UoW.Users.GetAsNoTrackingAsync(wrongExpr);
            });
        }

        [Fact]
        public async Task Repo_GenericAsync_Get_Success_Lambda()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var userExpr = new QueryExpression<Users>().Where(x => x.userID != Guid.NewGuid()).ToLambda();

            var users = await UoW.Users.GetAsync(userExpr);
            users = await UoW.Users.GetAsNoTrackingAsync(userExpr);
        }

        [Fact]
        public async Task Repo_GenericAsync_Get_Success_NoParam()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var users = await UoW.Users.GetAsync();
            users.Count().Should().Be(10);

            users = await UoW.Users.GetAsNoTrackingAsync();
            users.Count().Should().Be(10);
        }

        [Fact]
        public async Task Repo_GenericAsync_Update_Fail_Entities()
        {
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await UoW.DeleteDatasets();
                await UoW.CreateDatasets(10);

                await UoW.Users.UpdateAsync(
                    new List<Users>() {
                    new Users() { userID = Guid.NewGuid() },
                    new Users() { userID = Guid.NewGuid() },
                    });
                await UoW.CommitAsync();
            });
        }

        [Fact]
        public async Task Repo_GenericAsync_Update_Fail_Entity()
        {
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await UoW.DeleteDatasets();
                await UoW.CreateDatasets(10);

                await UoW.Users.UpdateAsync(new Users() { userID = Guid.NewGuid() });
                await UoW.CommitAsync();
            });
        }

        [Fact]
        public async Task Repo_GenericAsync_Update_Success_Entities()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var locations = await UoW.Locations.GetAsync();
            var location1 = locations.First();
            var location2 = locations.Last();

            var users = await UoW.Users.GetAsync();
            var user1 = users.First();
            var user2 = users.Last();

            user1.locationID = location2.locationID;
            user1.description = FakeConstants.TestDesc;

            user2.locationID = location1.locationID;
            user2.description = FakeConstants.TestDesc;

            await UoW.Users.UpdateAsync(new List<Users> { user1, user2 });
            await UoW.CommitAsync();
        }

        [Fact]
        public async Task Repo_GenericAsync_Update_Success_Entity()
        {
            await UoW.DeleteDatasets();
            await UoW.CreateDatasets(10);

            var location = (await UoW.Locations.GetAsync()).First();
            var user = (await UoW.Users.GetAsync()).First();

            user.locationID = location.locationID;
            user.description = FakeConstants.TestDesc;

            await UoW.Users.UpdateAsync(user);
            await UoW.CommitAsync();
        }
    }
}
