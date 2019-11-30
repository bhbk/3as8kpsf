using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Tests.Models
{
    public class SampleEntity
    {
        public SampleEntity()
        {
            child2 = new HashSet<SampleEntityChild>();
        }

        public bool bool1 { get; set; }
        public bool? bool2 { get; set; }
        public Guid guid1 { get; set; }
        public Guid? guid2 { get; set; }
        public DateTime date1 { get; set; }
        public DateTime? date2 { get; set; }
        public int int1 { get; set; }
        public int? int2 { get; set; }
        public decimal decimal1 { get; set; }
        public decimal? decimal2 { get; set; }
        public string string1 { get; set; }
        public virtual SampleEntityChild child1 { get; set; }
        public virtual ICollection<SampleEntityChild> child2 { get; set; }
    }

    public class SampleEntityChild
    {
        public Guid guid1 { get; set; }
        public DateTime date1 { get; set; }
        public int int1 { get; set; }
        public decimal decimal1 { get; set; }
        public string string1 { get; set; }
        public virtual SampleEntity parent1 { get; set; }
    }
}
