using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Interfaces
{
    public interface IDataStateResult<TEntity>
    {
        IEnumerable<TEntity> Data { get; set; }
        int Total { get; set; }
    }
}
