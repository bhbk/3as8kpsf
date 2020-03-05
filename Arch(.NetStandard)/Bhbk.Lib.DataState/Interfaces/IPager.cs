using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Interfaces
{
    public interface IPager
    {
        int Skip { get; set; }
        int Take { get; set; }
        string Filter { get; set; }
        ICollection<KeyValuePair<string, string>> Sort { get; set; }
    }
}
