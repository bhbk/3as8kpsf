using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataAccess.EF.Interfaces;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWork
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
