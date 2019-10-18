using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using System;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWork
{
    public class SampleUoW : ISampleUoW
    {
        private readonly SampleEntities _context;
        public InstanceContext InstanceType { get; }
        public IGenericRepository<Users> UserRepo { get; }
        public IGenericRepository<Roles> RoleRepo { get; }
        public IGenericRepository<Locations> LocationRepo { get; }

        public SampleUoW()
        {
            InstanceType = InstanceContext.UnitTest;

            //var connection = Effort.EntityConnectionFactory.CreateTransient("name=_DbContext");
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new SampleEntities(connection);

            UserRepo = new GenericRepository<Users>(_context);
            RoleRepo = new GenericRepository<Roles>(_context);
            LocationRepo = new GenericRepository<Locations>(_context);
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
