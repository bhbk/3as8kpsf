using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly TestUnitOfWork _unitOfWork;

        public UnitOfWorkTests()
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
                    description = $"User{i}",
                    int1 = i
                });
            }

            _context.SaveChanges();
        }

        #region Repository Factory Tests

        [Fact]
        public void Repository_ReturnsTypedRepository()
        {
            var userRepo = _unitOfWork.Repository<User>();

            userRepo.Should().NotBeNull();
            userRepo.Should().BeAssignableTo<IGenericRepository<User>>();
        }

        [Fact]
        public void Repository_ReturnsCachedInstance()
        {
            var repo1 = _unitOfWork.Repository<User>();
            var repo2 = _unitOfWork.Repository<User>();

            repo1.Should().BeSameAs(repo2);
        }

        [Fact]
        public void Repository_DifferentTypes_ReturnsDifferentInstances()
        {
            var userRepo = _unitOfWork.Repository<User>();
            var locationRepo = _unitOfWork.Repository<Location>();

            userRepo.Should().NotBeSameAs(locationRepo);
        }

        #endregion

        #region RepositoryAsync Factory Tests

        [Fact]
        public void RepositoryAsync_ReturnsTypedRepository()
        {
            var userRepo = _unitOfWork.RepositoryAsync<User>();

            userRepo.Should().NotBeNull();
            userRepo.Should().BeAssignableTo<IGenericRepositoryAsync<User>>();
        }

        [Fact]
        public void RepositoryAsync_ReturnsCachedInstance()
        {
            var repo1 = _unitOfWork.RepositoryAsync<User>();
            var repo2 = _unitOfWork.RepositoryAsync<User>();

            repo1.Should().BeSameAs(repo2);
        }

        #endregion

        #region Commit Tests

        [Fact]
        public void Commit_SavesChanges()
        {
            var location = _context.Locations.First();
            var newUser = new User
            {
                userID = Guid.NewGuid(),
                locationID = location.locationID,
                description = "NewUser"
            };

            var repo = _unitOfWork.Repository<User>();
            repo.Post(newUser);

            _unitOfWork.Commit();

            _context.Users.Count().Should().Be(6);
        }

        [Fact]
        public async Task CommitAsync_SavesChanges()
        {
            var location = _context.Locations.First();
            var newUser = new User
            {
                userID = Guid.NewGuid(),
                locationID = location.locationID,
                description = "AsyncNewUser"
            };

            var repo = _unitOfWork.RepositoryAsync<User>();
            await repo.PostAsync(newUser);

            await _unitOfWork.CommitAsync();

            (await _context.Users.CountAsync()).Should().Be(6);
        }

        #endregion

        #region DbContext Access Tests

        [Fact]
        public void DbContext_ReturnsContext()
        {
            var context = _unitOfWork.DbContext;

            context.Should().NotBeNull();
            context.Should().BeSameAs(_context);
        }

        #endregion

        #region InstanceContext Tests

        [Fact]
        public void InstanceType_ReturnsConfiguredValue()
        {
            var instanceType = _unitOfWork.InstanceType;

            instanceType.Should().Be(InstanceContext.UnitTest);
        }

        #endregion

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _context.Dispose();
            _connection.Dispose();
        }
    }

    public class TestUnitOfWork : GenericUnitOfWorkAsync<SampleEntities>
    {
        public TestUnitOfWork(SampleEntities context)
            : base(context, InstanceContext.UnitTest)
        {
        }
    }
}
