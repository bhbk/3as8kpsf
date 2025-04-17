using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    public class BulkOperationTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericRepository<User> _repository;
        private readonly Guid _locationId;

        public BulkOperationTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<SampleEntities>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new SampleEntities(options);
            _context.Database.EnsureCreated();

            _repository = new GenericRepository<User>(_context);
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
                    description = i < 5 ? "Active" : "Inactive",
                    int1 = i * 10
                });
            }

            _context.SaveChanges();
            return location.locationID;
        }

        [Fact]
        public void DeleteBulk_DeletesMatchingEntities()
        {
            var deleted = _repository.DeleteBulk(u => u.description == "Inactive");

            deleted.Should().Be(5);
            _context.Users.Count().Should().Be(5);
            _context.Users.All(u => u.description == "Active").Should().BeTrue();
        }

        [Fact]
        public void DeleteBulk_NoMatches_ReturnsZero()
        {
            var deleted = _repository.DeleteBulk(u => u.description == "NonExistent");

            deleted.Should().Be(0);
            _context.Users.Count().Should().Be(10);
        }

        [Fact]
        public void UpdateBulk_UpdatesMatchingEntities()
        {
            var updated = _repository.UpdateBulk(
                predicate: u => u.description == "Inactive",
                setPropertyCalls: s => s.SetProperty(u => u.description, "Archived"));

            updated.Should().Be(5);

            _context.ChangeTracker.Clear();
            _context.Users.Count(u => u.description == "Archived").Should().Be(5);
        }

        [Fact]
        public void UpdateBulk_MultipleProperties()
        {
            var updated = _repository.UpdateBulk(
                predicate: u => u.description == "Active",
                setPropertyCalls: s => s
                    .SetProperty(u => u.description, "UpdatedActive")
                    .SetProperty(u => u.int2, 999));

            updated.Should().Be(5);

            _context.ChangeTracker.Clear();
            var updatedUsers = _context.Users.Where(u => u.description == "UpdatedActive").ToList();
            updatedUsers.Count.Should().Be(5);
            updatedUsers.All(u => u.int2 == 999).Should().BeTrue();
        }

        [Fact]
        public void UpdateBulk_NoMatches_ReturnsZero()
        {
            var updated = _repository.UpdateBulk(
                predicate: u => u.description == "NonExistent",
                setPropertyCalls: s => s.SetProperty(u => u.description, "Changed"));

            updated.Should().Be(0);
        }

        [Fact]
        public void DeleteBulk_WithAllMatches_DeletesAll()
        {
            var deleted = _repository.DeleteBulk(u => true);

            deleted.Should().Be(10);
            _context.Users.Count().Should().Be(0);
        }

        [Fact]
        public void UpdateBulk_IncrementValue()
        {
            _context.Users.Where(u => u.int1 >= 100).Count().Should().Be(0);

            var updated = _repository.UpdateBulk(
                predicate: u => u.int1 < 10,
                setPropertyCalls: s => s.SetProperty(u => u.int1, u => u.int1 + 100));

            updated.Should().BeGreaterThan(0);
            _context.ChangeTracker.Clear();
            _context.Users.Where(u => u.int1 >= 100).Count().Should().Be(updated);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
