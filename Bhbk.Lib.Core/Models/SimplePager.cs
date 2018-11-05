using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.Core.Models
{
    public class SimplePager
    {
        public string Filter { get; set; }

        [Required]
        [MinLength(1)]
        public string OrderBy { get; set; }

        [Required]
        [RegularExpression("asc|desc")]    //require value of asc or desc
        public string Order { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$")]    //require integer value greater than 0
        public int Skip { get; set; }

        [Required]
        [RegularExpression("^[1-9][0-9]*$")]    //require integer value greater than 1
        public int Take { get; set; }

        public SimplePager() { }

        public SimplePager(string orderBy, int skip, int take)
        {
            Filter = string.Empty;
            Order = "asc";
            OrderBy = orderBy;
            Skip = skip;
            Take = take;
        }

        public SimplePager(string order, string orderBy, int skip, int take)
        {
            Filter = string.Empty;
            Order = order;
            OrderBy = orderBy;
            Skip = skip;
            Take = take;
        }

        public SimplePager(string filter, string order, string orderBy, int skip, int take)
        {
            Filter = filter;
            Order = order;
            OrderBy = orderBy;
            Skip = skip;
            Take = take;
        }
    }
}
