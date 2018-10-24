using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.Core.Models
{
    public class Paging
    {
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
            OrderBy = orderBy;
            Skip = skip;
            Take = take;
        }
    }
}
