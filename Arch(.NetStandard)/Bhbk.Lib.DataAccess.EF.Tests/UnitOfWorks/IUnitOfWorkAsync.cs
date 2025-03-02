using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
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
