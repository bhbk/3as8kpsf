using Bhbk.Lib.DataState.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    [Obsolete]
    public class CascadePager
    {
        [CascadePagerFilter]
        public string Filter { get; set; }

        [CascadePagerSort]
        public List<KeyValuePair<string, string>> Sort { get; set; }

        /*
         * require integer value of 0 or greater
         */
        [Required]
        [RegularExpression("^[0-9]*$")]
        public int Skip { get; set; }

        /*
         * require integer value of 1 or greater
         */
        [Required]
        [RegularExpression("^[1-9][0-9]*$")]
        public int Take { get; set; }
    }

    public class CascadePageResult<TEntity>
    {
        public IEnumerable<TEntity> List { get; set; }
        public int Count { get; set; }
    }
}
