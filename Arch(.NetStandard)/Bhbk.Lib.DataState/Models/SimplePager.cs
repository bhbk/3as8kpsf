using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    [Obsolete]
    public class SimplePager
    {
        public string Filter { get; set; }

        [Required]
        [MinLength(1)]
        public string OrderBy { get; set; }

        /*
         * require value of asc or desc
         */
        [Required]
        [RegularExpression("asc|desc")]
        public string Order { get; set; }

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
}
