using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Interfaces
{
    public interface IDataState
    {
        int Skip { get; set; }
        int Take { get; set; }
        IDataStateFilter Filter { get; set; }
        ICollection<IDataStateSort> Sort { get; set; }
    }
}
