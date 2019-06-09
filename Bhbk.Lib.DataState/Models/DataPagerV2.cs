using Bhbk.Lib.DataState.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    [Obsolete]
    public class DataPagerV2
    {
        [DataPagerFiltersV2]
        public string Filter { get; set; }

        [Required]
        [DataPagerSortsV2]
        public List<KeyValuePair<string, string>> Sort { get; set; }

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
    }
}
