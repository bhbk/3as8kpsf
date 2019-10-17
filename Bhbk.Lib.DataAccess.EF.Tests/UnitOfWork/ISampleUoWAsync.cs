using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataAccess.EF.Interfaces;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWork
{
    public interface ISampleUoWAsync : IGenericUnitOfWorkAsync
    {
        IGenericRepositoryAsync<Users> UserRepo { get; }
        IGenericRepositoryAsync<Roles> RoleRepo { get; }
        IGenericRepositoryAsync<Locations> LocationRepo { get; }
        Task DataCreate(int sets);
        Task DataDestroy();
    }
}
