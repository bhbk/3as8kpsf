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
    public class QueryEscapeHatchAsyncTests : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericRepositoryAsync<User> _repository;

        public QueryEscapeHatchAsyncTests()
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

            for (int i = 0; i < 5; i++)
            {
                _context.Users.Add(new User
                {
                    userID = Guid.NewGuid(),
                    locationID = location.locationID,
                    description = $"AsyncUser{i}",
                    int1 = i
                });
            }

            _context.SaveChanges();
        }

        [Fact]
        public async Task Query_CanBeUsedWithToListAsync()
        {
            var users = await _repository.Query()
                .Where(u => u.int1 >= 2)
                .ToListAsync();

            users.Count.Should().Be(3);
        }

        [Fact]
        public async Task Query_CanBeUsedWithFirstOrDefaultAsync()
        {
            var user = await _repository.Query()
                .Where(u => u.description == "AsyncUser0")
                .FirstOrDefaultAsync();

            user.Should().NotBeNull();
            user.int1.Should().Be(0);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
