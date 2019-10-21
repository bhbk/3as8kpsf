using Bhbk.Lib.DataState.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    public class PageStateTypeC
    {
        [PageStateTypeCFilter]
        public PageStateTypeCFilters Filter { get; set; }

        [PageStateTypeCSort]
        public List<PageStateTypeCSort> Sort { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public int Skip { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public int Take { get; set; }

        public class PageStateTypeCFilters : PageStateTypeCFilter
        {
            public string Logic { get; set; }
            public List<PageStateTypeCFilters> Filters { get; set; }
        }

        public class PageStateTypeCFilter
        {
            public string Field { get; set; }
            public string Operator { get; set; }
            public string Value { get; set; }
            public bool IgnoreCase { get; set; }
        }

        public class PageStateTypeCSort
        {
            public string Field { get; set; }
            public string Dir { get; set; }
        }
    }

    public class PageStateTypeCResult<TEntity>
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int Total { get; set; }
    }
}
