using Bhbk.Lib.DataState.Extensions;
using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.DataState.Tests.Models;
using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Factories;
using System;
using System.Collections.Generic;
using Xunit;
using Bhbk.Lib.DataState.Models;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class DataStateExpressionTests
    {
        #region Filter Success Tests

        [Theory]
        [InlineData("string1", "contains", "test")]
        [InlineData("string1", "doesnotcontain", "test")]
        [InlineData("string1", "startswith", "test")]
        [InlineData("string1", "doesnotstartswith", "test")]
        [InlineData("string1", "endswith", "test")]
        [InlineData("string1", "doesnotendwith", "test")]
        [InlineData("string1", "eq", "test")]
        [InlineData("string1", "neq", "test")]
        [InlineData("string1", "isnullorempty", null)]
        [InlineData("string1", "isnotnullorempty", null)]
        [InlineData("string1", "isnullorwhitespace", null)]
        [InlineData("int1", "eq", "1000")]
        [InlineData("int1", "neq", "1000")]
        [InlineData("int1", "gt", "1000")]
        [InlineData("int1", "gte", "1000")]
        [InlineData("int1", "lt", "1000")]
        [InlineData("int1", "lte", "1000")]
        [InlineData("decimal1", "eq", "100.50")]
        [InlineData("decimal1", "gt", "100.50")]
        [InlineData("bool1", "eq", "true")]
        [InlineData("bool1", "neq", "false")]
        [InlineData("guid1", "eq", "0bb2871b-0b3a-468c-baf1-c24ed66a7de1")]
        [InlineData("date1", "eq", "2020-01-01")]
        [InlineData("date1", "gt", "2020-01-01")]
        [InlineData("date1", "lt", "2020-01-01")]
        public void Expr_DataState_Success_SingleFilter(string field, string op, string value)
        {
            var state = new PagerState()
            {
                Filter = new PagerStateFilter()
                {
                    Logic = "and",
                    Filters = new List<IDataStateFilter>()
                    {
                        new PagerStateFilter { Field = field, Operator = op, Value = value }
                    }
                },
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = "string1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            Assert.NotNull(predicate);
        }

        [Theory]
        [InlineData("string1", "contains", "x", "int1", "eq", "1000")]
        [InlineData("string1", "startswith", "x", "int1", "gt", "500")]
        [InlineData("decimal1", "gte", "100", "bool1", "eq", "true")]
        [InlineData("guid1", "neq", "0bb2871b-0b3a-468c-baf1-c24ed66a7de1", "date1", "gt", "2020-01-01")]
        public void Expr_DataState_Success_MultiFilter(
            string field1, string op1, string value1,
            string field2, string op2, string value2)
        {
            var state = new PagerState()
            {
                Filter = new PagerStateFilter()
                {
                    Logic = "and",
                    Filters = new List<IDataStateFilter>()
                    {
                        new PagerStateFilter { Field = field1, Operator = op1, Value = value1 },
                        new PagerStateFilter { Field = field2, Operator = op2, Value = value2 }
                    }
                },
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = "string1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            Assert.NotNull(predicate);
        }

        [Theory]
        [InlineData("and", "or")]
        [InlineData("or", "and")]
        [InlineData("and", "and")]
        [InlineData("or", "or")]
        public void Expr_DataState_Success_NestedFilter(string outerLogic, string innerLogic)
        {
            var state = new PagerState()
            {
                Filter = new PagerStateFilter()
                {
                    Logic = outerLogic,
                    Filters = new List<IDataStateFilter>()
                    {
                        new PagerStateFilter { Field = "string1", Operator = "contains", Value = "x" },
                        new PagerStateFilter
                        {
                            Logic = innerLogic,
                            Filters = new List<IDataStateFilter>()
                            {
                                new PagerStateFilter { Field = "int1", Operator = "eq", Value = "1000" },
                                new PagerStateFilter { Field = "int1", Operator = "gt", Value = "500" }
                            }
                        }
                    }
                },
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = "string1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            Assert.NotNull(predicate);
            Assert.NotNull(expression);
        }

        #endregion

        #region Filter Fail Tests

        [Theory]
        [InlineData("invalidfield", "eq", "1000")]
        [InlineData("nonexistent", "contains", "test")]
        [InlineData("notreal", "startswith", "x")]
        public void Expr_DataState_Fail_Filter_InvalidField(string field, string op, string value)
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new PagerState()
                {
                    Filter = new PagerStateFilter()
                    {
                        Logic = "and",
                        Filters = new List<IDataStateFilter>()
                        {
                            new PagerStateFilter { Field = field, Operator = op, Value = value }
                        }
                    }
                };

                QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            });
        }

        [Theory]
        [InlineData("string1", "invalidop", "test")]
        [InlineData("int1", "badoperator", "1000")]
        [InlineData("decimal1", "notreal", "100.50")]
        public void Expr_DataState_Fail_Filter_InvalidOperator(string field, string op, string value)
        {
            Assert.Throws<QueryExpressionFilterException>(() =>
            {
                var state = new PagerState()
                {
                    Filter = new PagerStateFilter()
                    {
                        Logic = "and",
                        Filters = new List<IDataStateFilter>()
                        {
                            new PagerStateFilter { Field = field, Operator = op, Value = value }
                        }
                    }
                };

                QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            });
        }

        [Theory]
        [InlineData("int1", "eq", "notanumber")]
        [InlineData("decimal1", "gt", "invalid")]
        [InlineData("bool1", "eq", "notabool")]
        [InlineData("guid1", "eq", "notaguuid")]
        [InlineData("date1", "eq", "notadate")]
        public void Expr_DataState_Fail_Filter_InvalidValue(string field, string op, string value)
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var state = new PagerState()
                {
                    Filter = new PagerStateFilter()
                    {
                        Logic = "and",
                        Filters = new List<IDataStateFilter>()
                        {
                            new PagerStateFilter { Field = field, Operator = op, Value = value }
                        }
                    }
                };

                QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            });
        }

        [Theory]
        [InlineData("invalid", "eq", "1000", "int1", "eq", "1000")]
        [InlineData("int1", "eq", "1000", "invalid", "eq", "1000")]
        public void Expr_DataState_Fail_Filter_NestedInvalidField(
            string field1, string op1, string value1,
            string field2, string op2, string value2)
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new PagerState()
                {
                    Filter = new PagerStateFilter()
                    {
                        Logic = "and",
                        Filters = new List<IDataStateFilter>()
                        {
                            new PagerStateFilter { Field = field1, Operator = op1, Value = value1 },
                            new PagerStateFilter
                            {
                                Logic = "or",
                                Filters = new List<IDataStateFilter>()
                                {
                                    new PagerStateFilter { Field = field2, Operator = op2, Value = value2 }
                                }
                            }
                        }
                    }
                };

                QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            });
        }

        #endregion

        #region Sort Success Tests

        [Theory]
        [InlineData("guid1", "asc")]
        [InlineData("guid1", "desc")]
        [InlineData("guid2", "asc")]
        [InlineData("guid2", "desc")]
        [InlineData("date1", "asc")]
        [InlineData("date1", "desc")]
        [InlineData("date2", "asc")]
        [InlineData("date2", "desc")]
        [InlineData("int1", "asc")]
        [InlineData("int1", "desc")]
        [InlineData("int2", "asc")]
        [InlineData("int2", "desc")]
        [InlineData("decimal1", "asc")]
        [InlineData("decimal1", "desc")]
        [InlineData("decimal2", "asc")]
        [InlineData("decimal2", "desc")]
        [InlineData("bool1", "asc")]
        [InlineData("bool1", "desc")]
        [InlineData("bool2", "asc")]
        [InlineData("bool2", "desc")]
        [InlineData("string1", "asc")]
        [InlineData("string1", "desc")]
        public void Expr_DataState_Success_Sort(string field, string dir)
        {
            var state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field, Dir = dir }
                },
                Skip = 0,
                Take = 1000
            };

            var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            Assert.NotNull(expression);
        }

        [Theory]
        [InlineData("guid1", "asc", "int1", "desc")]
        [InlineData("string1", "asc", "date1", "desc")]
        [InlineData("decimal1", "desc", "bool1", "asc")]
        [InlineData("int1", "asc", "int2", "asc")]
        public void Expr_DataState_Success_MultiSort(
            string field1, string dir1,
            string field2, string dir2)
        {
            var state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field1, Dir = dir1 },
                    new PagerStateSort() { Field = field2, Dir = dir2 }
                },
                Skip = 0,
                Take = 1000
            };

            var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            Assert.NotNull(expression);
        }

        [Theory]
        [InlineData("guid1")]
        [InlineData("date1")]
        [InlineData("int1")]
        [InlineData("decimal1")]
        [InlineData("bool1")]
        [InlineData("string1")]
        public void Expr_DataState_Success_Sort_DefaultDir(string field)
        {
            var control = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field, Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field }
                },
                Skip = 0,
                Take = 1000
            };

            var validate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(control);
            var expression = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);

            Assert.Equal(validate.Body.ToString(), expression.Body.ToString());
        }

        #endregion

        #region Sort Fail Tests

        [Theory]
        [InlineData("invalidfield", "asc")]
        [InlineData("nonexistent", "desc")]
        [InlineData("notreal", "asc")]
        public void Expr_DataState_Fail_Sort_InvalidField(string field, string dir)
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new PagerState()
                {
                    Sort = new List<IDataStateSort>()
                    {
                        new PagerStateSort() { Field = field, Dir = dir }
                    },
                    Skip = 0,
                    Take = 1000
                };

                QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            });
        }

        [Theory]
        [InlineData("string1", "invalid")]
        [InlineData("int1", "ascending")]
        [InlineData("decimal1", "descending")]
        public void Expr_DataState_Fail_Sort_InvalidDir(string field, string dir)
        {
            Assert.Throws<QueryExpressionSortException>(() =>
            {
                var state = new PagerState()
                {
                    Sort = new List<IDataStateSort>()
                    {
                        new PagerStateSort() { Field = field, Dir = dir }
                    },
                    Skip = 0,
                    Take = 1000
                };

                QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            });
        }

        #endregion

        #region Skip/Take Fail Tests

        [Theory]
        [InlineData(-1, 1000)]
        [InlineData(-100, 1000)]
        [InlineData(-1, 100)]
        public void Expr_DataState_Fail_Skip(int skip, int take)
        {
            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var state = new PagerState()
                {
                    Sort = new List<IDataStateSort>()
                    {
                        new PagerStateSort() { Field = "string1", Dir = "asc" }
                    },
                    Skip = skip,
                    Take = take
                };

                QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            });
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, -1)]
        [InlineData(100, 0)]
        [InlineData(100, -1)]
        [InlineData(0, -100)]
        public void Expr_DataState_Fail_Take(int skip, int take)
        {
            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var state = new PagerState()
                {
                    Sort = new List<IDataStateSort>()
                    {
                        new PagerStateSort() { Field = "string1", Dir = "asc" }
                    },
                    Skip = skip,
                    Take = take
                };

                QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyState(state);
            });
        }

        #endregion

        #region Nullable Field Tests

        [Theory]
        [InlineData("int2", "eq", "1000")]
        [InlineData("int2", "neq", "500")]
        [InlineData("decimal2", "gt", "100.50")]
        [InlineData("decimal2", "lte", "200.00")]
        [InlineData("bool2", "eq", "true")]
        [InlineData("guid2", "eq", "0bb2871b-0b3a-468c-baf1-c24ed66a7de1")]
        [InlineData("date2", "gt", "2020-01-01")]
        public void Expr_DataState_Success_NullableField(string field, string op, string value)
        {
            var state = new PagerState()
            {
                Filter = new PagerStateFilter()
                {
                    Logic = "and",
                    Filters = new List<IDataStateFilter>()
                    {
                        new PagerStateFilter { Field = field, Operator = op, Value = value }
                    }
                },
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = "string1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            Assert.NotNull(predicate);
        }

        [Theory]
        [InlineData("int2", "eq", null)]
        [InlineData("decimal2", "eq", null)]
        [InlineData("bool2", "eq", null)]
        [InlineData("guid2", "eq", null)]
        [InlineData("date2", "eq", null)]
        public void Expr_DataState_Success_NullableField_NullValue(string field, string op, string value)
        {
            var state = new PagerState()
            {
                Filter = new PagerStateFilter()
                {
                    Logic = "and",
                    Filters = new List<IDataStateFilter>()
                    {
                        new PagerStateFilter { Field = field, Operator = op, Value = value }
                    }
                },
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = "string1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = QueryExpressionFactory.GetQueryExpression<DataStateModel>().ApplyPredicate(state);
            Assert.NotNull(predicate);
        }

        #endregion

    }
}
