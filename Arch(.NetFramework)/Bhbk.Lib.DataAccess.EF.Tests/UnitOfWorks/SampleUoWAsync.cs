using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using System;
using System.Threading.Tasks;
using FakeConstants = Bhbk.Lib.DataAccess.EF.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public class SampleUoWAsync : ISampleUoWAsync
    {
        private readonly SampleEntities _context;
        public InstanceContext InstanceType { get; }
        public IGenericRepositoryAsync<Users> Users { get; }
        public IGenericRepositoryAsync<Roles> Roles { get; }
        public IGenericRepositoryAsync<Locations> Locations { get; }

        public SampleUoWAsync()
        {
            InstanceType = InstanceContext.UnitTest;

            //var connection = Effort.EntityConnectionFactory.CreateTransient("name=SampleEntities");
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new SampleEntities(connection);

            Users = new GenericRepositoryAsync<Users>(_context);
            Roles = new GenericRepositoryAsync<Roles>(_context);
            Locations = new GenericRepositoryAsync<Locations>(_context);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CreateDatasets(int sets)
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

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDatasets()
        {
            _context.Set<Users>().RemoveRange(_context.Users);
            _context.Set<Roles>().RemoveRange(_context.Roles);
            _context.Set<Locations>().RemoveRange(_context.Locations);

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
