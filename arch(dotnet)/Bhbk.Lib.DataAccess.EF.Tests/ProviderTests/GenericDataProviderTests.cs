using Bhbk.Lib.DataAccess.EF.Providers;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataState.Interfaces;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Bhbk.Lib.DataState.Models;

namespace Bhbk.Lib.DataAccess.EF.Tests.ProviderTests
{
    public class GenericDataProviderTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericDataProvider<User> _provider;
        private readonly Guid _locationId;

        public GenericDataProviderTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<SampleEntities>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new SampleEntities(options);
            _context.Database.EnsureCreated();

            _provider = new GenericDataProvider<User>(_context);
            _locationId = SeedData();
        }

        private Guid SeedData()
        {
            var location = new Location { locationID = Guid.NewGuid(), description = "TestLocation" };
            _context.Locations.Add(location);

            for (int i = 0; i < 10; i++)
            {
                _context.Users.Add(new User
                {
                    userID = Guid.NewGuid(),
                    locationID = location.locationID,
                    description = $"User{i}",
                    int1 = i * 10,
                    date1 = DateTime.Now.AddDays(-i)
                });
            }

            _context.SaveChanges();
            return location.locationID;
        }

        #region Query Tests

        [Fact]
        public void Query_ReturnsIQueryable()
        {
            var query = _provider.Query();

            query.Should().NotBeNull();
            query.Count().Should().Be(10);
        }

        #endregion

        #region Find Tests

        [Fact]
        public void Find_ExistingEntity_ReturnsEntity()
        {
            var existingUser = _context.Users.First();

            var result = _provider.Find(existingUser.userID);

            result.Should().NotBeNull();
            result.userID.Should().Be(existingUser.userID);
        }

        [Fact]
        public void Find_NonExistingEntity_ReturnsNull()
        {
            var result = _provider.Find(Guid.NewGuid());

            result.Should().BeNull();
        }

        #endregion

        #region Select Tests

        [Fact]
        public void Select_NoPredicate_ReturnsAllEntities()
        {
            var result = _provider.Select();

            result.Should().HaveCount(10);
        }

        [Fact]
        public void Select_WithPredicate_FiltersEntities()
        {
            var result = _provider.Select(u => u.int1 > 50);

            result.Should().HaveCount(4);
            result.All(u => u.int1 > 50).Should().BeTrue();
        }

        [Fact]
        public void Select_WithDataState_ReturnsPagedResult()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 5,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "asc" }
                }
            };

            var result = _provider.Select(state);

            result.Data.Should().HaveCount(5);
            result.Total.Should().Be(10);
        }

        [Fact]
        public void Select_WithProjection_ReturnsProjectedResult()
        {
            var result = _provider.Select(
                selector: u => new { u.description, u.int1 });

            result.Should().HaveCount(10);
        }

        #endregion

        #region Post Tests

        [Fact]
        public void Post_NewEntity_AddsToDatabase()
        {
            var newUser = new User
            {
                userID = Guid.NewGuid(),
                locationID = _locationId,
                description = "NewUser",
                int1 = 100
            };

            var result = _provider.Post(newUser);

            result.Should().NotBeNull();
            _context.Users.Count().Should().Be(11);
        }

        [Fact]
        public void Post_WithCommitFalse_DoesNotSaveImmediately()
        {
            var newUser = new User
            {
                userID = Guid.NewGuid(),
                locationID = _locationId,
                description = "NoCommitUser"
            };

            var result = _provider.Post(newUser, commit: false);

            result.Should().NotBeNull();
            _context.ChangeTracker.HasChanges().Should().BeTrue();
        }

        #endregion

        #region Put Tests

        [Fact]
        public void Put_ExistingEntity_UpdatesInDatabase()
        {
            var user = _context.Users.First();
            user.description = "UpdatedDescription";

            var result = _provider.Put(user);

            result.description.Should().Be("UpdatedDescription");
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_ExistingEntity_RemovesFromDatabase()
        {
            var user = _context.Users.First();
            var initialCount = _context.Users.Count();

            _provider.Delete(user);

            _context.Users.Count().Should().Be(initialCount - 1);
        }

        [Fact]
        public void Delete_WithPredicate_RemovesMatchingEntities()
        {
            var initialCount = _context.Users.Count();

            _provider.Delete(u => u.int1 > 50);

            _context.Users.Count().Should().Be(initialCount - 4);
        }

        #endregion

        public void Dispose()
        {
            _provider.Dispose();
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
