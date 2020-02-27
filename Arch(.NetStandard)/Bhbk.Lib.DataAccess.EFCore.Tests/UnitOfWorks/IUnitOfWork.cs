using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Bhbk.Lib.DataAccess.EFCore.UnitOfWorks;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks
{
    public interface IUnitOfWork : IGenericUnitOfWork
    {
        IGenericRepository<Users> Users { get; }
        IGenericRepository<Roles> Roles { get; }
        IGenericRepository<Locations> Locations { get; }
        void CreateDatasets(int sets);
        void DeleteDatasets();
    }
}
