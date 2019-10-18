using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using System;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWork
{
    public class SampleUoWAsync : ISampleUoWAsync
    {
        private readonly SampleEntities _context;
        public InstanceContext InstanceType { get; }
        public IGenericRepositoryAsync<Users> UserRepo { get; }
        public IGenericRepositoryAsync<Roles> RoleRepo { get; }
        public IGenericRepositoryAsync<Locations> LocationRepo { get; }

        public SampleUoWAsync()
        {
            InstanceType = InstanceContext.UnitTest;

            //var connection = Effort.EntityConnectionFactory.CreateTransient("name=_DbContext");
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new SampleEntities(connection);

            UserRepo = new GenericRepositoryAsync<Users>(_context);
            RoleRepo = new GenericRepositoryAsync<Roles>(_context);
            LocationRepo = new GenericRepositoryAsync<Locations>(_context);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DataCreate(int sets)
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
            }

            await _context.SaveChangesAsync();
        }

        public async Task DataDestroy()
        {
            _context.Set<Users>().RemoveRange(_context.Users);
            _context.Set<Roles>().RemoveRange(_context.Roles);
            _context.Set<Locations>().RemoveRange(_context.Locations);

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.SaveChangesAsync();
        }
    }
}
