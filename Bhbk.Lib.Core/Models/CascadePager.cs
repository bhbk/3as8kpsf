using Bhbk.Lib.Core.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.Core.Models
{
    public class CascadePager
    {
        public string Filter { get; set; }

        [Required]
        [CascadePagerOrders]                    //require certain things in each tuple
        public List<Tuple<string, string>> Orders { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$")]         //require integer value greater than 0
        public int Skip { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]*$")]    //require integer value greater than 1
        public int Take { get; set; }
    }
}
