using Bhbk.Lib.DataAccess.EF.Repositories;
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

namespace Bhbk.Lib.DataAccess.EF.Tests.DataStateTests
{
    public class DataStateIntegrationTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly GenericRepository<User> _repository;

        public DataStateIntegrationTests()
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

            for (int i = 0; i < 25; i++)
            {
                _context.Users.Add(new User
                {
                    userID = Guid.NewGuid(),
                    locationID = location.locationID,
                    description = $"User{i:D2}",
                    int1 = i,
                    decimal1 = i * 10.5m,
                    date1 = DateTime.Now.AddDays(-i)
                });
            }

            _context.SaveChanges();
        }

        [Fact]
        public void Get_WithDataState_FirstPage()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 10,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "asc" }
                }
            };

            var result = _repository.Get(state);

            result.Should().NotBeNull();
            result.Data.Should().HaveCount(10);
            result.Total.Should().Be(25);
            result.Data.First().int1.Should().Be(0);
        }

        [Fact]
        public void Get_WithDataState_SecondPage()
        {
            var state = new PagerState
            {
                Skip = 10,
                Take = 10,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "asc" }
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().HaveCount(10);
            result.Total.Should().Be(25);
            result.Data.First().int1.Should().Be(10);
        }

        [Fact]
        public void Get_WithDataState_LastPage()
        {
            var state = new PagerState
            {
                Skip = 20,
                Take = 10,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "asc" }
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().HaveCount(5);
            result.Total.Should().Be(25);
        }

        [Fact]
        public void Get_WithDataState_SortAscending()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 25,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "asc" }
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().BeInAscendingOrder(u => u.int1);
        }

        [Fact]
        public void Get_WithDataState_SortDescending()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 25,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "desc" }
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().BeInDescendingOrder(u => u.int1);
        }

        [Fact]
        public void Get_WithDataState_FilterEquals()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 25,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "asc" }
                },
                Filter = new PagerStateFilter
                {
                    Field = "int1",
                    Operator = "eq",
                    Value = "5"
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().HaveCount(1);
            result.Total.Should().Be(1);
            result.Data.First().int1.Should().Be(5);
        }

        [Fact]
        public void Get_WithDataState_FilterGreaterThan()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 25,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "asc" }
                },
                Filter = new PagerStateFilter
                {
                    Field = "int1",
                    Operator = "gt",
                    Value = "20"
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().HaveCount(4);
            result.Total.Should().Be(4);
            result.Data.All(u => u.int1 > 20).Should().BeTrue();
        }

        [Fact]
        public void Get_WithDataState_FilterContains()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 25,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "description", Dir = "asc" }
                },
                Filter = new PagerStateFilter
                {
                    Field = "description",
                    Operator = "contains",
                    Value = "User0"
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().HaveCount(10);
            result.Data.All(u => u.description.Contains("User0")).Should().BeTrue();
        }

        [Fact]
        public void Get_WithDataState_FilterSortAndPage()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 5,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "int1", Dir = "desc" }
                },
                Filter = new PagerStateFilter
                {
                    Field = "int1",
                    Operator = "gte",
                    Value = "10"
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().HaveCount(5);
            result.Total.Should().Be(15);
            result.Data.Should().BeInDescendingOrder(u => u.int1);
            result.Data.First().int1.Should().Be(24);
        }

        [Fact]
        public void Get_WithDataState_MultiSort()
        {
            var state = new PagerState
            {
                Skip = 0,
                Take = 25,
                Sort = new List<IDataStateSort>
                {
                    new PagerStateSort { Field = "description", Dir = "asc" },
                    new PagerStateSort { Field = "int1", Dir = "desc" }
                }
            };

            var result = _repository.Get(state);

            result.Data.Should().HaveCount(25);
            result.Total.Should().Be(25);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
