using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataAccess.EF.UnitOfWorks;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public interface IUnitOfWork : IGenericUnitOfWork
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<Location> Locations { get; }
        void CreateDatasets(int sets);
        void DeleteDatasets();
    }
}
