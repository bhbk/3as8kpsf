using Bhbk.Lib.Core.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.Core.Models
{
    public class TuplePager
    {
        public string Filter { get; set; }

        [Required]
        [ListOfTuples]
        public List<Tuple<string, string>> Orders { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$")]    //require integer value greater than 0
        public int Skip { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]*$")]    //require integer value greater than 1
        public int Take { get; set; }

        public TuplePager() { }

        public TuplePager(List<Tuple<string, string>> orders, int skip, int take)
        {
            Filter = string.Empty;
            Orders = orders;
            Skip = skip;
            Take = take;
        }

        public TuplePager(string filter, List<Tuple<string, string>> orders, int skip, int take)
        {
            Filter = filter;
            Orders = orders;
            Skip = skip;
            Take = take;
        }
    }
}
