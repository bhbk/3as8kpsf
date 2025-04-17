using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    public class BulkOperationAsyncTests : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericRepositoryAsync<User> _repository;

        public BulkOperationAsyncTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<SampleEntities>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new SampleEntities(options);
            _context.Database.EnsureCreated();

            _repository = new GenericRepositoryAsync<User>(_context);
            SeedData();
        }

        private void SeedData()
        {
            var location = new Location { locationID = Guid.NewGuid(), description = "TestLocation" };
            _context.Locations.Add(location);

            for (int i = 0; i < 10; i++)
            {
                _context.Users.Add(new User
                {
                    userID = Guid.NewGuid(),
                    locationID = location.locationID,
                    description = i < 5 ? "Active" : "Inactive",
                    int1 = i * 10
                });
            }

            _context.SaveChanges();
        }

        [Fact]
        public async Task DeleteBulkAsync_DeletesMatchingEntities()
        {
            var deleted = await _repository.DeleteBulkAsync(u => u.description == "Inactive");

            deleted.Should().Be(5);
            (await _context.Users.CountAsync()).Should().Be(5);
        }

        [Fact]
        public async Task DeleteBulkAsync_NoMatches_ReturnsZero()
        {
            var deleted = await _repository.DeleteBulkAsync(u => u.description == "NonExistent");

            deleted.Should().Be(0);
        }

        [Fact]
        public async Task UpdateBulkAsync_UpdatesMatchingEntities()
        {
            var updated = await _repository.UpdateBulkAsync(
                predicate: u => u.description == "Inactive",
                setPropertyCalls: s => s.SetProperty(u => u.description, "Archived"));

            updated.Should().Be(5);
        }

        [Fact]
        public async Task UpdateBulkAsync_NoMatches_ReturnsZero()
        {
            var updated = await _repository.UpdateBulkAsync(
                predicate: u => u.description == "NonExistent",
                setPropertyCalls: s => s.SetProperty(u => u.description, "Changed"));

            updated.Should().Be(0);
        }

        [Fact]
        public async Task DeleteBulkAsync_WithAllMatches_DeletesAll()
        {
            var deleted = await _repository.DeleteBulkAsync(u => true);

            deleted.Should().Be(10);
            (await _context.Users.CountAsync()).Should().Be(0);
        }

        [Fact]
        public async Task UpdateBulkAsync_IncrementValue()
        {
            var updated = await _repository.UpdateBulkAsync(
                predicate: u => u.int1 < 50,
                setPropertyCalls: s => s.SetProperty(u => u.int1, u => u.int1 + 100));

            updated.Should().BeGreaterThan(0);
            _context.ChangeTracker.Clear();
            (await _context.Users.Where(u => u.int1 >= 100).CountAsync()).Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task DeleteBulkAsync_WithCancellationToken_Success()
        {
            var cts = new CancellationTokenSource();
            var deleted = await _repository.DeleteBulkAsync(u => u.description == "Inactive", cts.Token);

            deleted.Should().Be(5);
        }

        [Fact]
        public async Task UpdateBulkAsync_WithCancellationToken_Success()
        {
            var cts = new CancellationTokenSource();
            var updated = await _repository.UpdateBulkAsync(
                predicate: u => u.description == "Active",
                setPropertyCalls: s => s.SetProperty(u => u.description, "Updated"),
                ct: cts.Token);

            updated.Should().Be(5);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
