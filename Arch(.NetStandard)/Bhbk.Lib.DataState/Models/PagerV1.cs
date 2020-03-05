using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    [Obsolete]
    public class PagerV1
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

    [Obsolete]
    public class PagerV1Result<TEntity>
    {
        public IEnumerable<TEntity> List { get; set; }
        public int Count { get; set; }
    }
}
