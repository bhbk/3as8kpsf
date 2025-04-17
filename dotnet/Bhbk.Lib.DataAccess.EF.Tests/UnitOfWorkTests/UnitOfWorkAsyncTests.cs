using Bhbk.Lib.DataAccess.EF.Tests.Models;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public class UnitOfWorkAsyncTests : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly TestUnitOfWork _unitOfWork;

        public UnitOfWorkAsyncTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<SampleEntities>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new SampleEntities(options);
            _context.Database.EnsureCreated();

            _unitOfWork = new TestUnitOfWork(_context);
        }

        [Fact]
        public async Task DisposeAsync_DisposesCorrectly()
        {
            var location = new Location { locationID = Guid.NewGuid(), description = "Test" };
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            await _unitOfWork.DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
