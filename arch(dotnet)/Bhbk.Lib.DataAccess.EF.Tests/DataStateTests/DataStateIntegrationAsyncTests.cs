using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataState.Interfaces;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Bhbk.Lib.DataState.Models;

namespace Bhbk.Lib.DataAccess.EF.Tests.DataStateTests
{
    public class DataStateIntegrationAsyncTests : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericRepositoryAsync<User> _repository;

        public DataStateIntegrationAsyncTests()
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

            for (int i = 0; i < 20; i++)
            {
                _context.Users.Add(new User
                {
                    userID = Guid.NewGuid(),
                    locationID = location.locationID,
                    description = $"AsyncUser{i:D2}",
                    int1 = i
                });
            }

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAsync_WithDataState_ReturnsPaged()
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

            var result = await _repository.GetAsync(state);

            result.Data.Should().HaveCount(5);
            result.Total.Should().Be(20);
        }

        [Fact]
        public async Task GetAsync_WithDataState_FilterAndSort()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 20,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "desc" }
                },
                Filter = new PagerStateFilter
                {
                    Field = "int1",
                    Operator = "lt",
                    Value = "10"
                }
            };

            var result = await _repository.GetAsync(state);

            result.Data.Should().HaveCount(10);
            result.Total.Should().Be(10);
            result.Data.Should().BeInDescendingOrder(u => u.int1);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
