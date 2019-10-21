using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    [Obsolete]
    public class PageStateTypeA
    {
        public string Filter { get; set; }

        [Required]
        [MinLength(1)]
        public string OrderBy { get; set; }

        [Required]
        [RegularExpression("asc|desc")]
        public string Order { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public int Skip { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public int Take { get; set; }
    }
}
