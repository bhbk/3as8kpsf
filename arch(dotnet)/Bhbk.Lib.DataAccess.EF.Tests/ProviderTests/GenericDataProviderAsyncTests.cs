using Bhbk.Lib.DataAccess.EF.Providers;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataState.Interfaces;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Bhbk.Lib.DataState.Models;

namespace Bhbk.Lib.DataAccess.EF.Tests.ProviderTests
{
    public class GenericDataProviderAsyncTests : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericDataProviderAsync<User> _provider;
        private readonly Guid _locationId;

        public GenericDataProviderAsyncTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<SampleEntities>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new SampleEntities(options);
            _context.Database.EnsureCreated();

            _provider = new GenericDataProviderAsync<User>(_context);
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
                    description = $"AsyncUser{i}",
                    int1 = i * 10
                });
            }

            _context.SaveChanges();
            return location.locationID;
        }

        #region FindAsync Tests

        [Fact]
        public async Task FindAsync_ExistingEntity_ReturnsEntity()
        {
            var existingUser = _context.Users.First();

            var result = await _provider.FindAsync(new object[] { existingUser.userID });

            result.Should().NotBeNull();
            result.userID.Should().Be(existingUser.userID);
        }

        [Fact]
        public async Task FindAsync_NonExistingEntity_ReturnsNull()
        {
            var result = await _provider.FindAsync(new object[] { Guid.NewGuid() });

            result.Should().BeNull();
        }

        #endregion

        #region SelectAsync Tests

        [Fact]
        public async Task SelectAsync_NoPredicate_ReturnsAllEntities()
        {
            var result = await _provider.SelectAsync();

            result.Should().HaveCount(10);
        }

        [Fact]
        public async Task SelectAsync_WithPredicate_FiltersEntities()
        {
            var result = await _provider.SelectAsync(u => u.int1 > 50);

            result.Should().HaveCount(4);
        }

        [Fact]
        public async Task SelectAsync_WithDataState_ReturnsPagedResult()
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

            var result = await _provider.SelectAsync(state);

            result.Data.Should().HaveCount(5);
            result.Total.Should().Be(10);
        }

        [Fact]
        public async Task SelectAsync_WithProjection_ReturnsProjectedResult()
        {
            var result = await _provider.SelectAsync(
                selector: u => new { u.description, u.int1 });

            result.Should().HaveCount(10);
        }

        #endregion

        #region PostAsync Tests

        [Fact]
        public async Task PostAsync_NewEntity_AddsToDatabase()
        {
            var newUser = new User
            {
                userID = Guid.NewGuid(),
                locationID = _locationId,
                description = "NewAsyncUser"
            };

            var result = await _provider.PostAsync(newUser);

            result.Should().NotBeNull();
            (await _context.Users.CountAsync()).Should().Be(11);
        }

        [Fact]
        public async Task PostAsync_WithCancellationToken()
        {
            var newUser = new User
            {
                userID = Guid.NewGuid(),
                locationID = _locationId,
                description = "CancellableUser"
            };
            var cts = new CancellationTokenSource();

            var result = await _provider.PostAsync(newUser, ct: cts.Token);

            result.Should().NotBeNull();
        }

        #endregion

        #region PutAsync Tests

        [Fact]
        public async Task PutAsync_ExistingEntity_UpdatesInDatabase()
        {
            var user = _context.Users.First();
            user.description = "UpdatedAsyncDescription";

            var result = await _provider.PutAsync(user);

            result.description.Should().Be("UpdatedAsyncDescription");
        }

        #endregion

        #region DeleteAsync Tests

        [Fact]
        public async Task DeleteAsync_ExistingEntity_RemovesFromDatabase()
        {
            var user = _context.Users.First();
            var initialCount = await _context.Users.CountAsync();

            await _provider.DeleteAsync(user);

            (await _context.Users.CountAsync()).Should().Be(initialCount - 1);
        }

        [Fact]
        public async Task DeleteAsync_WithPredicate_RemovesMatchingEntities()
        {
            var initialCount = await _context.Users.CountAsync();

            await _provider.DeleteAsync(u => u.int1 > 50);

            (await _context.Users.CountAsync()).Should().Be(initialCount - 4);
        }

        #endregion

        #region Cancellation Tests

        [Fact]
        public async Task SelectAsync_WithCancellationToken_PassesToken()
        {
            var cts = new CancellationTokenSource();

            var result = await _provider.SelectAsync(ct: cts.Token);

            result.Should().HaveCount(10);
        }

        [Fact]
        public async Task DeleteAsync_WithCancellationToken_PassesToken()
        {
            var user = _context.Users.First();
            var cts = new CancellationTokenSource();

            await _provider.DeleteAsync(user, ct: cts.Token);

            // Assert - no exception thrown
        }

        #endregion

        public async ValueTask DisposeAsync()
        {
            await _provider.DisposeAsync();
            await _context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
