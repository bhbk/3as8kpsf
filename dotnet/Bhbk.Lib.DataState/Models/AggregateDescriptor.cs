using Bhbk.Lib.DataState.Interfaces;

namespace Bhbk.Lib.DataState.Models
{
    public class AggregateDescriptor : IDataStateAggregate
    {
        public string Field { get; set; }
        public string Aggregate { get; set; }
    }
}
