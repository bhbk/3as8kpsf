using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Models
{
    public class DataResultV3<TEntity>
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int Total { get; set; }
    }
}
