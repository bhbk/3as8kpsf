using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Interfaces
{
    public interface IDataStateGroup
    {
        string Field { get; set; }
        string Dir { get; set; }
        ICollection<IDataStateAggregate> Aggregates { get; set; }
    }
}
