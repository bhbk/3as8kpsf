using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWork
{
    public class SampleUoW : ISampleUoW
    {
        private readonly SampleEntities _context;
        public InstanceContext InstanceType { get; }
        public IGenericRepository<Users> UserRepo { get; private set; }
        public IGenericRepository<Roles> RoleRepo { get; private set; }
        public IGenericRepository<Locations> LocationRepo { get; private set; }

        public SampleUoW()
        {
            InstanceType = InstanceContext.UnitTest;

            var options = new DbContextOptionsBuilder<SampleEntities>()
                .EnableSensitiveDataLogging();

            InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(options, ":InMemory:");

            _context = new SampleEntities(options.Options);

            UserRepo = new GenericRepository<Users>(_context, InstanceContext.UnitTest);
            RoleRepo = new GenericRepository<Roles>(_context, InstanceContext.UnitTest);
            LocationRepo = new GenericRepository<Locations>(_context, InstanceContext.UnitTest);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void DataCreate(int sets)
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
                    int1 = 1000,
                    date1 = DateTime.Now,
                    decimal1 = 1000,
                });

                _context.Set<Roles>().Add(new Roles()
                {
                    roleID = roleKey,
                });

                _context.SaveChanges();
            }
        }

        public void DataDestroy()
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
    }
}
