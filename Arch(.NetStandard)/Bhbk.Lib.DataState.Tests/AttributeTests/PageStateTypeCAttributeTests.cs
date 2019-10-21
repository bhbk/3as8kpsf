using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using static Bhbk.Lib.DataState.Models.PageStateTypeC;

namespace Bhbk.Lib.DataState.Tests.AttributeTests
{
    public class PageStateTypeCAttributeTests
    {
        [Fact]
        public void Attr_PageStateTypeC_Fail_Filter()
        {
            var state = new PageStateTypeC()
            {
                Filter = new PageStateTypeCFilters { },
                Sort = new List<PageStateTypeCSort>()
                {
                    new PageStateTypeCSort() { Field = "field1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_PageStateTypeC_Fail_Sort()
        {
            var state = new PageStateTypeC()
            {
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_PageStateTypeC_Fail_Sort_Skip()
        {
            var state = new PageStateTypeC()
            {
                Sort = new List<PageStateTypeCSort>()
                {
                    new PageStateTypeCSort() { Field = "field1", Dir = "asc" }
                },
                Skip = -1,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_PageStateTypeC_Fail_Sort_Take()
        {
            var state = new PageStateTypeC()
            {
                Sort = new List<PageStateTypeCSort>()
                {
                    new PageStateTypeCSort() { Field = "field1", Dir = "asc" }
                },
                Skip = 0,
                Take = 0
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_PageStateTypeC_Success_Filter()
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

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Fact]
        public void Attr_PageStateTypeC_Success_Sort()
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

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Fact]
        public void Attr_PageStateTypeC_Success_Sort_NoDir()
        {
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

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }
    }
}
