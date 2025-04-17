using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EF.Tests.RepositoryTests
{
    public class ProjectionAsyncTests : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericRepositoryAsync<User> _repository;

        public ProjectionAsyncTests()
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
                    description = $"AsyncUser{i}",
                    int1 = i * 10
                });
            }

            _context.SaveChanges();
        }

        [Fact]
        public async Task SelectAsync_WithSelector_ReturnsProjectedResults()
        {
            var results = await _repository.SelectAsync(
                selector: u => new { u.description, u.int1 });

            results.Should().HaveCount(10);
        }

        [Fact]
        public async Task SelectAsync_WithSelectorAndPredicate_FiltersAndProjects()
        {
            var results = await _repository.SelectAsync(
                selector: u => u.int1,
                predicate: u => u.int1 > 50);

            results.Should().HaveCount(4);
            results.All(i => i > 50).Should().BeTrue();
        }

        [Fact]
        public async Task SelectAsync_WithSelector_EmptyResult()
        {
            var results = await _repository.SelectAsync(
                selector: u => u.description,
                predicate: u => u.int1 > 1000);

            results.Should().BeEmpty();
        }

        [Fact]
        public async Task SelectAsync_ProjectToSingleProperty()
        {
            var results = await _repository.SelectAsync(
                selector: u => u.userID);

            results.Should().HaveCount(10);
            results.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task SelectAsync_ProjectToCalculatedValue()
        {
            var results = await _repository.SelectAsync(
                selector: u => u.int1 * 2,
                predicate: u => u.int1 > 0);

            results.Should().HaveCount(9);
            results.All(r => r > 0 && r % 2 == 0).Should().BeTrue();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
