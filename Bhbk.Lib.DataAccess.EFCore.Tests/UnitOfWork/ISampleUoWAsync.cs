using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Bhbk.Lib.DataAccess.EFCore.Interfaces;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWork
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
