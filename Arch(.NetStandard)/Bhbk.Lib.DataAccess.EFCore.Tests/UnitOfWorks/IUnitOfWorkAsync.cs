using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Bhbk.Lib.DataAccess.EFCore.UnitOfWorks;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks
{
    public interface IUnitOfWorkAsync : IGenericUnitOfWorkAsync
    {
        IGenericRepositoryAsync<User> Users { get; }
        IGenericRepositoryAsync<Role> Roles { get; }
        IGenericRepositoryAsync<Location> Locations { get; }
        ValueTask CreateDatasets(int sets);
        ValueTask DeleteDatasets();
    }
}
