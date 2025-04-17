using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeConstants = Bhbk.Lib.DataAccess.EF.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync
    {
        private readonly SqliteConnection _connection;
        private readonly SampleEntities _context;
        private readonly Dictionary<Type, object> _repositories;
        private readonly Dictionary<Type, object> _repositoriesAsync;

        public InstanceContext InstanceType { get; private set; }
        public DbContext DbContext => _context;
        public IGenericRepositoryAsync<User> Users { get; private set; }
        public IGenericRepositoryAsync<Role> Roles { get; private set; }
        public IGenericRepositoryAsync<Location> Locations { get; private set; }

        public UnitOfWorkAsync()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<SampleEntities>()
                .EnableSensitiveDataLogging()
                .UseSqlite(_connection);

            _context = new SampleEntities(options.Options);
            _context.Database.EnsureCreated();
            _repositories = new Dictionary<Type, object>();
            _repositoriesAsync = new Dictionary<Type, object>();

            InstanceType = InstanceContext.UnitTest;

            Users = new GenericRepositoryAsync<User>(_context);
            Roles = new GenericRepositoryAsync<Role>(_context);
            Locations = new GenericRepositoryAsync<Location>(_context);
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var entityType = typeof(TEntity);

            if (!_repositories.TryGetValue(entityType, out var repository))
            {
                repository = new GenericRepository<TEntity>(_context);
                _repositories[entityType] = repository;
            }

            return (IGenericRepository<TEntity>)repository;
        }

        public IGenericRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class
        {
            var entityType = typeof(TEntity);

            if (!_repositoriesAsync.TryGetValue(entityType, out var repository))
            {
                repository = new GenericRepositoryAsync<TEntity>(_context);
                _repositoriesAsync[entityType] = repository;
            }

            return (IGenericRepositoryAsync<TEntity>)repository;
        }

        public void Commit(int? timeout = null)
        {
            if (timeout.HasValue)
                _context.Database.SetCommandTimeout(timeout.Value);

            _context.SaveChanges();
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async ValueTask CreateDatasets(int sets)
        {
            for (int i = 0; i < sets; i++)
            {
                var locationKey = Guid.NewGuid();
                var userKey = Guid.NewGuid();
                var roleKey = Guid.NewGuid();

                _context.Set<Location>().Add(new Location()
                {
                    locationID = locationKey,
                });

                _context.Set<User>().Add(new User()
                {
                    userID = userKey,
                    locationID = locationKey,
                    int1 = FakeConstants.TestInteger,
                    date1 = DateTime.Now,
                    decimal1 = FakeConstants.TestDecimal,
                });

                _context.Set<Role>().Add(new Role()
                {
                    roleID = roleKey,
                });
            }

            await _context.SaveChangesAsync();
        }

        public async ValueTask DeleteDatasets()
        {
            _context.Set<User>().RemoveRange(_context.Users);
            _context.Set<Role>().RemoveRange(_context.Roles);
            _context.Set<Location>().RemoveRange(_context.Locations);

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            _connection.Close();
            await _connection.DisposeAsync();
        }
    }
}
