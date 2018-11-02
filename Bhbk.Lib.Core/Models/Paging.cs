using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Bhbk.Lib.Core.Validators;

namespace Bhbk.Lib.Core.Models
{
    public class Paging
    {
        public string Filter { get; private set; }

        [Required]
        [RegularExpression("asc|desc")]    //require integer value greater than 1
        public string Order { get; private set; }

        //[RequiredStringArray]
        //public string [] OrderBy { get; private set; }
        [Required]
        [MinLength(1)]
        public string OrderBy { get; private set; }

        [Required]
        [RegularExpression("^[0-9]*$")]    //require integer value greater than 0
        public int Skip { get; private set; }

        [Required]
        [RegularExpression("^[1-9][0-9]*$")]    //require integer value greater than 1
        public int Take { get; private set; }

        public Paging(string orderBy, int skip, int take)
        {
            Filter = string.Empty;
            Order = "asc";
            OrderBy = orderBy;
            Skip = skip;
            Take = take;
        }

        public Paging(string order, string orderBy, int skip, int take)
        {
            Filter = string.Empty;
            Order = order;
            OrderBy = orderBy;
            Skip = skip;
            Take = take;
        }

        public Paging(string filter, string order, string orderBy, int skip, int take)
        {
            Filter = filter;
            Order = order;
            OrderBy = orderBy;
            Skip = skip;
            Take = take;
        }
    }
}
