using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataState.Tests.Models
{
    public class TestModel
    {
        public TestModel()
        {
            child1 = new HashSet<TestModelChild>();
            child2 = new HashSet<TestModelChild>();
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
        public virtual ICollection<TestModelChild> child1 { get; set; }
        public virtual ICollection<TestModelChild> child2 { get; set; }
    }

    public class TestModelChild
    {
        public virtual TestModel parent1 { get; set; }
        public virtual TestModel parent2 { get; set; }
    }
}
