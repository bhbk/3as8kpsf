using Bhbk.Lib.DataState.Attributes;
using Bhbk.Lib.DataState.Converters;
using Bhbk.Lib.DataState.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bhbk.Lib.DataState.Models
{
    [Serializable]
    [DataStateV1]
    public class DataStateV1 : IDataState
    {
        [JsonConverter(typeof(ModelConverter<DataStateV1Filter, IDataStateFilter>))]
        public IDataStateFilter Filter { get; set; }

        [JsonConverter(typeof(CollectionModelConverter<DataStateV1Sort, IDataStateSort>))]
        public ICollection<IDataStateSort> Sort { get; set; }

        [Range(0, long.MaxValue)]
        public int Skip { get; set; }

        [Range(1, short.MaxValue)]
        public int Take { get; set; }
    }

    public class DataStateV1Filter : IDataStateFilter
    {
        public string Logic { get; set; }
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public bool IgnoreCase { get; set; }

        [JsonConverter(typeof(CollectionModelConverter<DataStateV1Filter, IDataStateFilter>))]
        public ICollection<IDataStateFilter> Filters { get; set; }
    }

    public class DataStateV1Sort : IDataStateSort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }

    public class DataStateV1Result<TEntity> : IDataStateResult<TEntity>
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int Total { get; set; }
    }
}
