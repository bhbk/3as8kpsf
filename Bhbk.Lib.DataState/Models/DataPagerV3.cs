using Bhbk.Lib.DataState.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    public class DataPagerV3
    {
        [DataPagerFiltersV3]
        public List<FilterDescriptor> Filter { get; set; }

        [Required]
        [DataPagerSortsV3]
        public List<SortDescriptor> Sort { get; set; }

        /*
         * require integer value greater than 0
         */
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int Skip { get; set; }

        /*
         * require integer value greater than 1
         */
        [Required]
        [RegularExpression("^[1-9][0-9]*$")]
        public int Take { get; set; }

        public class FilterDescriptor
        {
            public string Field { get; set; }
            public string Operator { get; set; }
            public string Value { get; set; }
            public bool IgnoreCase { get; set; }
        }

        public class SortDescriptor
        {
            public string Field { get; set; }
            public string Dir { get; set; }
        }
    }
}
