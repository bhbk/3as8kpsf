using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Interfaces;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWork
{
    public interface ISampleUoW : IGenericUnitOfWork
    {
        IGenericRepository<Users> UserRepo { get; }
        IGenericRepository<Roles> RoleRepo { get; }
        IGenericRepository<Locations> LocationRepo { get; }
        void DataCreate(int sets);
        void DataDestroy();
    }
}
