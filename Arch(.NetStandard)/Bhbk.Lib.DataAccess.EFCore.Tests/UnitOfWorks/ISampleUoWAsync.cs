using Bhbk.Lib.DataAccess.EFCore.Repositories;
using Bhbk.Lib.DataAccess.EFCore.Tests.Models;
using Bhbk.Lib.DataAccess.EFCore.UnitOfWorks;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.UnitOfWorks
{
    public interface ISampleUoWAsync : IGenericUnitOfWorkAsync
    {
        IGenericRepositoryAsync<Users> Users { get; }
        IGenericRepositoryAsync<Roles> Roles { get; }
        IGenericRepositoryAsync<Locations> Locations { get; }
        ValueTask CreateDatasets(int sets);
        ValueTask DeleteDatasets();
    }
}
