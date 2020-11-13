using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FakeConstants = Bhbk.Lib.DataAccess.EFCore.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync
    {
        private readonly SampleEntities _context;
        public InstanceContext InstanceType { get; private set; }
        public IGenericRepositoryAsync<User> Users { get; private set; }
        public IGenericRepositoryAsync<Role> Roles { get; private set; }
        public IGenericRepositoryAsync<Location> Locations { get; private set; }

        public UnitOfWorkAsync()
        {
            var options = new DbContextOptionsBuilder<SampleEntities>()
                .EnableSensitiveDataLogging();

            InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(options, ":InMemory:");

            _context = new SampleEntities(options.Options);

            InstanceType = InstanceContext.UnitTest;

            Users = new GenericRepositoryAsync<User>(_context);
            Roles = new GenericRepositoryAsync<Role>(_context);
            Locations = new GenericRepositoryAsync<Location>(_context);
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

        public async ValueTask CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
