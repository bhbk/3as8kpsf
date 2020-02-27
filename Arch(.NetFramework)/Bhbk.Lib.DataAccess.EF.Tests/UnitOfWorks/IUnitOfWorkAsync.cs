using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.Tests.Models;
using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Tests.UnitOfWorks
{
    public interface IUnitOfWorkAsync : IGenericUnitOfWorkAsync
    {
        IGenericRepositoryAsync<Users> Users { get; }
        IGenericRepositoryAsync<Roles> Roles { get; }
        IGenericRepositoryAsync<Locations> Locations { get; }
        Task CreateDatasets(int sets);
        Task DeleteDatasets();
    }
}
