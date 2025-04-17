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
    public class ProjectionTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericRepository<User> _repository;

        public ProjectionTests()
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
                    description = $"User{i}",
                    int1 = i * 10,
                    decimal1 = i * 100.5m,
                    date1 = DateTime.Now.AddDays(-i)
                });
            }

            _context.SaveChanges();
        }

        [Fact]
        public void Select_WithSelector_ReturnsProjectedResults()
        {
            var results = _repository.Select(
                selector: u => new { u.description, u.int1 });

            results.Should().HaveCount(10);
            results.First().Should().NotBeNull();
        }

        [Fact]
        public void Select_WithSelectorAndPredicate_FiltersAndProjects()
        {
            var results = _repository.Select(
                selector: u => new { u.description, u.int1 },
                predicate: u => u.int1 > 50);

            results.Should().HaveCount(4); // 60, 70, 80, 90
            results.All(r => r.int1 > 50).Should().BeTrue();
        }

        [Fact]
        public void Select_WithSelector_EmptyResult()
        {
            var results = _repository.Select(
                selector: u => u.description,
                predicate: u => u.int1 > 1000);

            results.Should().BeEmpty();
        }

        [Fact]
        public void Select_WithComplexProjection()
        {
            var results = _repository.Select(
                selector: u => new UserSummary
                {
                    Id = u.userID,
                    DisplayName = u.description,
                    Score = u.int1
                });

            results.Should().HaveCount(10);
            results.All(r => r.DisplayName.StartsWith("User")).Should().BeTrue();
        }

        [Fact]
        public void Select_ProjectToSingleProperty()
        {
            var results = _repository.Select(
                selector: u => u.userID);

            results.Should().HaveCount(10);
            results.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Select_ProjectToCalculatedValue()
        {
            var results = _repository.Select(
                selector: u => u.int1 * 2,
                predicate: u => u.int1 > 0);

            results.Should().HaveCount(9);
            results.All(r => r > 0 && r % 2 == 0).Should().BeTrue();
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }

        private class UserSummary
        {
            public Guid Id { get; set; }
            public string DisplayName { get; set; }
            public int Score { get; set; }
        }
    }
}
