using System;
using System.Collections.Generic;

namespace Bhbk.Lib.QueryExpression.Tests.Models
{
    public class QueryExpressionModel
    {
        public QueryExpressionModel()
        {
            child2 = new HashSet<QueryExpressionChildModel>();
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
        public virtual QueryExpressionChildModel child1 { get; set; }
        public virtual ICollection<QueryExpressionChildModel> child2 { get; set; }
    }

    public class QueryExpressionChildModel
    {
        public Guid guid1 { get; set; }
        public DateTime date1 { get; set; }
        public int int1 { get; set; }
        public decimal decimal1 { get; set; }
        public string string1 { get; set; }
        public virtual QueryExpressionModel parent1 { get; set; }
    }
}
