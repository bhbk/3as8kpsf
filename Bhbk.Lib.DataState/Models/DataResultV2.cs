using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Models
{
    public class DataResultV2<TEntity>
    {
        public IEnumerable<TEntity> List { get; set; }
        public int Count { get; set; }
    }
}
