using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using FakeConstants = Bhbk.Lib.DataAccess.EFCore.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SampleEntities _context;
        public InstanceContext InstanceType { get; private set; }
        public IGenericRepository<Users> Users { get; private set; }
        public IGenericRepository<Roles> Roles { get; private set; }
        public IGenericRepository<Locations> Locations { get; private set; }

        public UnitOfWork()
        {
            var options = new DbContextOptionsBuilder<SampleEntities>()
                .UseInMemoryDatabase(":InMemory:");

            _context = new SampleEntities(options.Options);

            InstanceType = InstanceContext.UnitTest;

            Users = new GenericRepository<Users>(_context);
            Roles = new GenericRepository<Roles>(_context);
            Locations = new GenericRepository<Locations>(_context);
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

                _context.Set<Locations>().Add(new Locations()
                {
                    locationID = locationKey,
                });

                _context.Set<Users>().Add(new Users()
                {
                    userID = userKey,
                    locationID = locationKey,
                    int1 = FakeConstants.TestInteger,
                    date1 = DateTime.Now,
                    decimal1 = FakeConstants.TestDecimal,
                });

                _context.Set<Roles>().Add(new Roles()
                {
                    roleID = roleKey,
                });
            }

            _context.SaveChanges();
        }

        public void DeleteDatasets()
        {
            _context.Set<Users>().RemoveRange(_context.Users);
            _context.Set<Roles>().RemoveRange(_context.Roles);
            _context.Set<Locations>().RemoveRange(_context.Locations);

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }
    }
}
