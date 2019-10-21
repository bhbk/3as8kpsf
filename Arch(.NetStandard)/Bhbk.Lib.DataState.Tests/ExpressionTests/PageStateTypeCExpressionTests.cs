using Bhbk.Lib.Cryptography.Entropy;
using Bhbk.Lib.DataState.Expressions;
using Bhbk.Lib.DataState.Models;
using Bhbk.Lib.DataState.Tests.Models;
using System.Collections.Generic;
using Xunit;
using static Bhbk.Lib.DataState.Models.PageStateTypeC;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class PageStateTypeCExpressionTests
    {
        [Fact]
        public void Expr_PageStateTypeC_Fail_Filter()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new PageStateTypeC()
                {
                    Filter = new PageStateTypeCFilters()
                    {
                        Logic = "and",
                        Filters = new List<PageStateTypeCFilters>()
                        {
                            new PageStateTypeCFilters { Field = AlphaNumeric.CreateString(8), Operator = "contains", Value = "1000" },
                            new PageStateTypeCFilters {
                                Logic = "or",
                                Filters = new List<PageStateTypeCFilters>()
                                {
                                    new PageStateTypeCFilters { Field = "int1", Operator = "eq", Value = "1000 "},
                                }
                            }
                        }
                    }
                };

                var predicate = state.ToPredicateExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_PageStateTypeC_Fail_Sort()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new PageStateTypeC()
                {
                    Sort = new List<PageStateTypeCSort>()
                    {
                        new PageStateTypeCSort() { Field = AlphaNumeric.CreateString(8), Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });

            Assert.Throws<QueryExpressionSortException>(() =>
            {
                var state = new PageStateTypeC()
                {
                    Sort = new List<PageStateTypeCSort>()
                    {
                        new PageStateTypeCSort() { Field = "string1", Dir = AlphaNumeric.CreateString(8) },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_PageStateTypeC_Fail_Sort_Skip()
        {
            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var state = new PageStateTypeC()
                {
                    Sort = new List<PageStateTypeCSort>()
                    {
                        new PageStateTypeCSort() { Field = "string1", Dir = "asc" },
                    },
                    Skip = -1,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_PageStateTypeC_Fail_Sort_Take()
        {
            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var state = new PageStateTypeC()
                {
                    Sort = new List<PageStateTypeCSort>()
                    {
                        new PageStateTypeCSort() { Field = "string1", Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 0
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_PageStateTypeC_Success_Filter()
        {
            var state = new PageStateTypeC()
            {
                Filter = new PageStateTypeCFilters()
                {
                    Logic = "and",
                    Filters = new List<PageStateTypeCFilters>()
                    {
                        new PageStateTypeCFilters { Field = "string1", Operator = "contains", Value = "x" },
                        new PageStateTypeCFilters { Field = "string1", Operator = "startswith", Value = "y" },
                        new PageStateTypeCFilters {
                            Logic = "or",
                            Filters = new List<PageStateTypeCFilters>()
                            {
                                new PageStateTypeCFilters { Field = "int1", Operator = "eq", Value = "1000 "},
                                new PageStateTypeCFilters { Field = "int1", Operator = "gt", Value = "1000 "},
                            }
                        }
                    }
                },
                Sort = new List<PageStateTypeCSort>()
                {
                    new PageStateTypeCSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = state.ToPredicateExpression<SampleEntity>();
            var expression = state.ToExpression<SampleEntity>();
        }

        [Fact]
        public void Expr_PageStateTypeC_Success_Sort()
        {
            var state = new PageStateTypeC()
            {
                Sort = new List<PageStateTypeCSort>()
                {
                    new PageStateTypeCSort() { Field = "guid1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "guid2", Dir = "desc" },
                    new PageStateTypeCSort() { Field = "date1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "date2", Dir = "desc" },
                    new PageStateTypeCSort() { Field = "int1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "int2", Dir = "desc" },
                    new PageStateTypeCSort() { Field = "decimal1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "decimal2", Dir = "desc" },
                    new PageStateTypeCSort() { Field = "bool1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "bool2", Dir = "desc" },
                    new PageStateTypeCSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var expression = state.ToExpression<SampleEntity>();
        }

        [Fact]
        public void Expr_PageStateTypeC_Success_Sort_NoDir()
        {
            var control = new PageStateTypeC()
            {
                Sort = new List<PageStateTypeCSort>()
                {
                    new PageStateTypeCSort() { Field = "guid1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "guid2", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "date1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "date2", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "int1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "int2", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "decimal1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "decimal2", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "bool1", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "bool2", Dir = "asc" },
                    new PageStateTypeCSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var state = new PageStateTypeC()
            {
                Sort = new List<PageStateTypeCSort>()
                {
                    new PageStateTypeCSort() { Field = "guid1" },
                    new PageStateTypeCSort() { Field = "guid2" },
                    new PageStateTypeCSort() { Field = "date1" },
                    new PageStateTypeCSort() { Field = "date2" },
                    new PageStateTypeCSort() { Field = "int1" },
                    new PageStateTypeCSort() { Field = "int2" },
                    new PageStateTypeCSort() { Field = "decimal1" },
                    new PageStateTypeCSort() { Field = "decimal2" },
                    new PageStateTypeCSort() { Field = "bool1" },
                    new PageStateTypeCSort() { Field = "bool2" },
                    new PageStateTypeCSort() { Field = "string1" },
                },
                Skip = 0,
                Take = 1000
            };

            var validate = control.ToExpression<SampleEntity>();
            var expression = state.ToExpression<SampleEntity>();

            Assert.Equal(validate.Body.ToString(), expression.Body.ToString());
        }
    }
}
