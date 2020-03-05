using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Interfaces
{
    public interface IPageResult<TEntity>
    {
        IEnumerable<TEntity> List { get; set; }
        int Count { get; set; }
    }
}
