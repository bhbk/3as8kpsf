using Bhbk.Lib.DataState.Attributes;
using Bhbk.Lib.DataState.Converters;
using Bhbk.Lib.DataState.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bhbk.Lib.DataState.Models
{
    [Serializable]
    [DataState]
    public class PagerState : IDataState
    {
        [JsonConverter(typeof(ModelConverter<PagerStateFilter, IDataStateFilter>))]
        public IDataStateFilter Filter { get; set; }

        [JsonConverter(typeof(CollectionModelConverter<PagerStateSort, IDataStateSort>))]
        public ICollection<IDataStateSort> Sort { get; set; }

        [Range(0, long.MaxValue)]
        public int Skip { get; set; }

        [Range(1, short.MaxValue)]
        public int Take { get; set; }
    }

    public class PagerStateFilter : IDataStateFilter
    {
        public string Logic { get; set; }
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public bool IgnoreCase { get; set; }

        [JsonConverter(typeof(CollectionModelConverter<PagerStateFilter, IDataStateFilter>))]
        public ICollection<IDataStateFilter> Filters { get; set; }
    }

    public class PagerStateSort : IDataStateSort
    {
        public string Field { get; set; }
        public string Dir { get; set; }

        public static ICollection<IDataStateSort> Get(ICollection<IDataStateSort> sortDescriptors)
        {
            var result = sortDescriptors ?? new List<IDataStateSort>();

            result = result
                .Where(x => !string.IsNullOrWhiteSpace(x.Field))
                .Where(x => !string.IsNullOrWhiteSpace(x.Dir))
                .ToList();

            return result;
        }

        public static bool TryParse(string field, string direction, out ICollection<IDataStateSort> result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(direction))
                return false;

            if (!direction.Equals("asc") && !direction.Equals("desc"))
                return false;

            result = new List<IDataStateSort>
            {
                new PagerStateSort { Field = field, Dir = direction }
            };

            return true;
        }

        public static bool TryParse(ICollection<IDataStateSort> sortDescriptors, out ICollection<IDataStateSort> result)
        {
            result = null;

            var filtered = Get(sortDescriptors);

            if (filtered.Any(x => !x.Dir.Equals("asc") && !x.Dir.Equals("desc")))
                return false;

            result = filtered;

            return true;
        }
    }

    public class PagerStateResult<TEntity> : IDataStateResult<TEntity>
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int Total { get; set; }
    }
}
