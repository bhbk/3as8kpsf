using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Microsoft.EntityFrameworkCore;
using System;
using FakeConstants = Bhbk.Lib.DataAccess.EFCore.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SampleEntities _context;
        public InstanceContext InstanceType { get; private set; }
        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<Role> Roles { get; private set; }
        public IGenericRepository<Location> Locations { get; private set; }

        public UnitOfWork()
        {
            var options = new DbContextOptionsBuilder<SampleEntities>()
                .UseInMemoryDatabase(":InMemory:");

            _context = new SampleEntities(options.Options);

            InstanceType = InstanceContext.UnitTest;

            Users = new GenericRepository<User>(_context);
            Roles = new GenericRepository<Role>(_context);
            Locations = new GenericRepository<Location>(_context);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void CreateDatasets(int sets)
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

            _context.SaveChanges();
        }

        public void DeleteDatasets()
        {
            _context.Set<User>().RemoveRange(_context.Users);
            _context.Set<Role>().RemoveRange(_context.Roles);
            _context.Set<Location>().RemoveRange(_context.Locations);

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
