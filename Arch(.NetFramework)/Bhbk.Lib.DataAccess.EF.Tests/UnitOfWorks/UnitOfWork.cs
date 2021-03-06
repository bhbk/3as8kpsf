﻿using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using System;
using FakeConstants = Bhbk.Lib.DataAccess.EF.Tests.Primitives.Constants;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SampleEntities _context;
        public InstanceContext InstanceType { get; private set; }
        public IGenericRepository<Users> Users { get; }
        public IGenericRepository<Roles> Roles { get; }
        public IGenericRepository<Locations> Locations { get; }

        public UnitOfWork()
        {
            //var connection = Effort.EntityConnectionFactory.CreateTransient("name=SampleEntities");
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new SampleEntities(connection);

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
    }
}
