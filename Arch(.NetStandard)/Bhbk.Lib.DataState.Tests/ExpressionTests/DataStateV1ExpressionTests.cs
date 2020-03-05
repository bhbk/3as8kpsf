using Bhbk.Lib.Cryptography.Entropy;
using Bhbk.Lib.DataState.Extensions;
using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.DataState.Models;
using Bhbk.Lib.DataState.Tests.Models;
using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Factories;
using System.Collections.Generic;
using Xunit;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class DataStateV1ExpressionTests
    {
        //[Theory]
        //[InlineData("invalid", "eq", "1000", "int1", "eq", "1000")]
        //[InlineData("int1", "invalid", "1000", "int1", "eq", "1000")]
        //[InlineData("int1", "eq", "1000", "invalid", "eq", "1000")]
        //[InlineData("int1", "eq", "1000", "int1", "invalid", "1000")]
        //[InlineData("int1", "eq", "1000", "int1", "eq", "invalid")]
        //[InlineData("int1", "contains", "1000", "int1", "eq", "1000")]
        //[InlineData("int1", "eq", "1000", "int1", "contains", "1000")]
        //[InlineData(null, "eq", "1000", "int1", "eq", "1000")]
        //[InlineData(null, null, "1000", "int1", "eq", "1000")]
        //[InlineData(null, null, null, "int1", "eq", "1000")]
        //[InlineData(null, null, null, null, "eq", "1000")]
        //[InlineData(null, null, null, null, null, "1000")]
        //[InlineData(null, null, null, null, null, null)]
        //[InlineData("int1", null, "1000", "int1", "eq", "1000")]
        //[InlineData("int1", "eq", null, "int1", "eq", "1000")]
        //[InlineData("int1", "eq", "1000", null, "eq", "1000")]
        //[InlineData("int1", "eq", "1000", "int1", null, "1000")]
        //[InlineData("int1", "eq", "1000", "int1", "eq", null)]
        //public void Expr_DataStateV1_Fail_Filter(string field1, string op1, string value1, string field2, string op2, string value2)
        //{
        //    Assert.Throws<QueryExpressionPropertyException>(() =>
        //    {
        //        var state = new DataStateV1()
        //        {
        //            Filter = new DataStateV1Filter()
        //            {
        //                Logic = "and",
        //                Filters = new List<IDataStateFilter>()
        //                {
        //                    new DataStateV1Filter { Field = field1, Operator = op1, Value = value1 },
        //                    new DataStateV1Filter {
        //                        Logic = "or",
        //                        Filters = new List<IDataStateFilter>()
        //                        {
        //                            new DataStateV1Filter { Field = field2, Operator = op2, Value = value2},
        //                        }
        //                    }
        //                }
        //            }
        //        };

        //        var predicate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
        //    });
        //}

        [Fact]
        public void Expr_DataStateV1_Fail_Filter()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new DataStateV1()
                {
                    Filter = new DataStateV1Filter()
                    {
                        Logic = "and",
                        Filters = new List<IDataStateFilter>()
                        {
                            new DataStateV1Filter { Field = AlphaNumeric.CreateString(8), Operator = "contains", Value = "1000" },
                            new DataStateV1Filter {
                                Logic = "or",
                                Filters = new List<IDataStateFilter>()
                                {
                                    new DataStateV1Filter { Field = "int1", Operator = "eq", Value = "1000 "},
                                }
                            }
                        }
                    }
                };

                var predicate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            });
        }

        [Fact]
        public void Expr_DataStateV1_Fail_Sort()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new DataStateV1()
                {
                    Sort = new List<IDataStateSort>()
                    {
                        new DataStateV1Sort() { Field = AlphaNumeric.CreateString(8), Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            });

            Assert.Throws<QueryExpressionSortException>(() =>
            {
                var state = new DataStateV1()
                {
                    Sort = new List<IDataStateSort>()
                    {
                        new DataStateV1Sort() { Field = "string1", Dir = AlphaNumeric.CreateString(8) },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            });
        }

        [Fact]
        public void Expr_DataStateV1_Fail_Sort_Skip()
        {
            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var state = new DataStateV1()
                {
                    Sort = new List<IDataStateSort>()
                    {
                        new DataStateV1Sort() { Field = "string1", Dir = "asc" },
                    },
                    Skip = -1,
                    Take = 1000
                };

                var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            });
        }

        [Fact]
        public void Expr_DataStateV1_Fail_Sort_Take()
        {
            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var state = new DataStateV1()
                {
                    Sort = new List<IDataStateSort>()
                    {
                        new DataStateV1Sort() { Field = "string1", Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 0
                };

                var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            });
        }

        [Fact]
        public void Expr_DataStateV1_Success_Filter()
        {
            var state = new DataStateV1()
            {
                Filter = new DataStateV1Filter()
                {
                    Logic = "and",
                    Filters = new List<IDataStateFilter>()
                    {
                        new DataStateV1Filter { Field = "string1", Operator = "contains", Value = "x" },
                        new DataStateV1Filter { Field = "string1", Operator = "startswith", Value = "y" },
                        new DataStateV1Filter {
                            Logic = "or",
                            Filters = new List<IDataStateFilter>()
                            {
                                new DataStateV1Filter { Field = "int1", Operator = "eq", Value = "1000 "},
                                new DataStateV1Filter { Field = "int1", Operator = "gt", Value = "1000 "},
                            }
                        }
                    }
                },
                Sort = new List<IDataStateSort>()
                {
                    new DataStateV1Sort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
        }

        [Fact]
        public void Expr_DataStateV1_Success_Sort()
        {
            var state = new DataStateV1()
            {
                Sort = new List<IDataStateSort>()
                {
                    new DataStateV1Sort() { Field = "guid1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "guid2", Dir = "desc" },
                    new DataStateV1Sort() { Field = "date1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "date2", Dir = "desc" },
                    new DataStateV1Sort() { Field = "int1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "int2", Dir = "desc" },
                    new DataStateV1Sort() { Field = "decimal1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "decimal2", Dir = "desc" },
                    new DataStateV1Sort() { Field = "bool1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "bool2", Dir = "desc" },
                    new DataStateV1Sort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
        }

        [Fact]
        public void Expr_DataStateV1_Success_Sort_NoDir()
        {
            var control = new DataStateV1()
            {
                Sort = new List<IDataStateSort>()
                {
                    new DataStateV1Sort() { Field = "guid1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "guid2", Dir = "asc" },
                    new DataStateV1Sort() { Field = "date1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "date2", Dir = "asc" },
                    new DataStateV1Sort() { Field = "int1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "int2", Dir = "asc" },
                    new DataStateV1Sort() { Field = "decimal1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "decimal2", Dir = "asc" },
                    new DataStateV1Sort() { Field = "bool1", Dir = "asc" },
                    new DataStateV1Sort() { Field = "bool2", Dir = "asc" },
                    new DataStateV1Sort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var state = new DataStateV1()
            {
                Sort = new List<IDataStateSort>()
                {
                    new DataStateV1Sort() { Field = "guid1" },
                    new DataStateV1Sort() { Field = "guid2" },
                    new DataStateV1Sort() { Field = "date1" },
                    new DataStateV1Sort() { Field = "date2" },
                    new DataStateV1Sort() { Field = "int1" },
                    new DataStateV1Sort() { Field = "int2" },
                    new DataStateV1Sort() { Field = "decimal1" },
                    new DataStateV1Sort() { Field = "decimal2" },
                    new DataStateV1Sort() { Field = "bool1" },
                    new DataStateV1Sort() { Field = "bool2" },
                    new DataStateV1Sort() { Field = "string1" },
                },
                Skip = 0,
                Take = 1000
            };

            var validate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(control);
            var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);

            Assert.Equal(validate.Body.ToString(), expression.Body.ToString());
        }
    }
}
