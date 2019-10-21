using Bhbk.Lib.DataState.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    [Obsolete]
    public class PageStateTypeB
    {
        public string Filter { get; set; }

        [PageStateTypeBSort]
        public List<KeyValuePair<string, string>> Sort { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public int Skip { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public int Take { get; set; }
    }

    public class PageStateTypeBResult<TEntity>
    {
        public IEnumerable<TEntity> List { get; set; }
        public int Count { get; set; }
    }
}
