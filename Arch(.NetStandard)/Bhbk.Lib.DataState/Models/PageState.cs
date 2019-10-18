using Bhbk.Lib.DataState.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    public class PageState
    {
        [PageStateFilter]
        public RecursiveFilterModel Filter { get; set; }

        [PageStateSort]
        public List<SortModel> Sort { get; set; }

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

        public class RecursiveFilterModel : FilterModel
        {
            public string Logic { get; set; }
            public List<RecursiveFilterModel> Filters { get; set; }
        }

        public class FilterModel
        {
            public string Field { get; set; }
            public string Operator { get; set; }
            public string Value { get; set; }
            public bool IgnoreCase { get; set; }
        }

        public class SortModel
        {
            public string Field { get; set; }
            public string Dir { get; set; }
        }
    }

    public class PageStateResult<TEntity>
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int Total { get; set; }
    }
}
