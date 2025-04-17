using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Interfaces
{
    public interface IDataStateFilter
    {
        string Logic { get; set; }
        string Field { get; set; }
        string Operator { get; set; }
        string Value { get; set; }
        bool IgnoreCase { get; set; }
        ICollection<IDataStateFilter> Filters { get; set; }
    }
}
