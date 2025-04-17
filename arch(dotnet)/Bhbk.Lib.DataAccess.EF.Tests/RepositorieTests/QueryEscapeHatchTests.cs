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
    public class QueryEscapeHatchTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericRepository<User> _repository;

        public QueryEscapeHatchTests()
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
            var location1 = new Location { locationID = Guid.NewGuid(), description = "Location1" };
            var location2 = new Location { locationID = Guid.NewGuid(), description = "Location2" };

            _context.Locations.AddRange(location1, location2);

            for (int i = 0; i < 10; i++)
            {
                _context.Users.Add(new User
                {
                    userID = Guid.NewGuid(),
                    locationID = i < 5 ? location1.locationID : location2.locationID,
                    description = $"User{i}",
                    int1 = i,
                    date1 = DateTime.Now.AddDays(-i)
                });
            }

            _context.SaveChanges();
        }

        [Fact]
        public void Query_ReturnsIQueryable_WithNoTracking()
        {
            var query = _repository.Query(asNoTracking: true);

            query.Should().NotBeNull();
            query.Should().BeAssignableTo<IQueryable<User>>();
            query.Count().Should().Be(10);
        }

        [Fact]
        public void Query_ReturnsIQueryable_WithTracking()
        {
            var query = _repository.Query(asNoTracking: false);
            var user = query.First();
            user.description = "Modified";

            _context.Entry(user).State.Should().Be(EntityState.Modified);
        }

        [Fact]
        public void Query_AllowsWhereClause()
        {
            var users = _repository.Query()
                .Where(u => u.int1 > 5)
                .ToList();

            users.Count.Should().Be(4);
            users.All(u => u.int1 > 5).Should().BeTrue();
        }

        [Fact]
        public void Query_AllowsNestedIncludes()
        {
            var users = _repository.Query()
                .Include(u => u.location)
                .Where(u => u.location != null)
                .ToList();

            users.Should().NotBeEmpty();
            users.All(u => u.location != null).Should().BeTrue();
        }

        [Fact]
        public void Query_AllowsOrderBy()
        {
            var users = _repository.Query()
                .OrderBy(u => u.int1)
                .ToList();

            users.Should().BeInAscendingOrder(u => u.int1);
        }

        [Fact]
        public void Query_AllowsProjection()
        {
            var descriptions = _repository.Query()
                .Select(u => u.description)
                .ToList();

            descriptions.Count.Should().Be(10);
            descriptions.All(d => d.StartsWith("User")).Should().BeTrue();
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
