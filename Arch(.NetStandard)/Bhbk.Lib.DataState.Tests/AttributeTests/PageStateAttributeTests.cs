using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using static Bhbk.Lib.DataState.Models.PageState;

namespace Bhbk.Lib.DataState.Tests.AttributeTests
{
    public class PageStateAttributeTests
    {
        [Fact]
        public void Attr_PageState_Fail_Fields_Filter()
        {
            var state = new PageState()
            {
                Filter = new PageStateFilters { },
                Sort = new List<PageStateSort>()
                {
                    new PageStateSort() { Field = "field1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_PageState_Fail_Fields_Sort()
        {
            var state = new PageState()
            {
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_PageState_Fail_Skip()
        {
            var state = new PageState()
            {
                Sort = new List<PageStateSort>()
                {
                    new PageStateSort() { Field = "field1", Dir = "asc" }
                },
                Skip = -1000,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_PageState_Fail_Take()
        {
            var state = new PageState()
            {
                Sort = new List<PageStateSort>()
                {
                    new PageStateSort() { Field = "field1", Dir = "asc" }
                },
                Skip = 0,
                Take = 0
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_PageState_Success()
        {
            var state = new PageState()
            {
                Filter = new PageStateFilters()
                {
                    Logic = "and",
                    Filters = new List<PageStateFilters>()
                    {
                        new PageStateFilters { Field = "string1", Operator = "contains", Value = "x" },
                        new PageStateFilters { Field = "string1", Operator = "startswith", Value = "y" },
                        new PageStateFilters {
                            Logic = "or",
                            Filters = new List<PageStateFilters>()
                            {
                                new PageStateFilters { Field = "int1", Operator = "eq", Value = "1000 "},
                                new PageStateFilters { Field = "int1", Operator = "gt", Value = "1000 "},
                            }
                        }
                    }
                },
                Sort = new List<PageStateSort>()
                {
                    new PageStateSort() { Field = "guid1", Dir = "asc" },
                    new PageStateSort() { Field = "guid2", Dir = "asc" },
                    new PageStateSort() { Field = "date1", Dir = "asc" },
                    new PageStateSort() { Field = "date2", Dir = "asc" },
                    new PageStateSort() { Field = "int1", Dir = "asc" },
                    new PageStateSort() { Field = "int2", Dir = "asc" },
                    new PageStateSort() { Field = "decimal1", Dir = "asc" },
                    new PageStateSort() { Field = "decimal2", Dir = "asc" },
                    new PageStateSort() { Field = "bool1", Dir = "asc" },
                    new PageStateSort() { Field = "bool2", Dir = "asc" },
                    new PageStateSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(actual);
        }
    }
}
