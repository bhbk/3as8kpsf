using Bhbk.Lib.DataState.Interfaces;
using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Models
{
    public class GroupDescriptor : IDataStateGroup
    {
        public string Field { get; set; }
        public string Dir { get; set; }
        public ICollection<IDataStateAggregate> Aggregates { get; set; }

        public GroupDescriptor()
        {
            Aggregates = new HashSet<IDataStateAggregate>();
        }
    }
}
