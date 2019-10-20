using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataAccess.EF.UnitOfWorks;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public interface ISampleUoW : IGenericUnitOfWork
    {
        IGenericRepository<Users> Users { get; }
        IGenericRepository<Roles> Roles { get; }
        IGenericRepository<Locations> Locations { get; }
        void CreateDatasets(int sets);
        void DeleteDatasets();
    }
}
