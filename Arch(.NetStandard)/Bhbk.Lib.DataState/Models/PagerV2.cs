using Bhbk.Lib.DataState.Attributes;
using Bhbk.Lib.DataState.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    [Obsolete]
    [Serializable]
    [PagerV2]
    public class PagerV2 : IPager
    {
        public string Filter { get; set; }

        public ICollection<KeyValuePair<string, string>> Sort { get; set; }

        [Range(0, long.MaxValue)]
        public int Skip { get; set; }

        [Range(1, short.MaxValue)]
        public int Take { get; set; }
    }

    [Obsolete]
    public class PagerV2Result<TEntity> : IPageResult<TEntity>
    {
        public IEnumerable<TEntity> List { get; set; }
        public int Count { get; set; }
    }
}
