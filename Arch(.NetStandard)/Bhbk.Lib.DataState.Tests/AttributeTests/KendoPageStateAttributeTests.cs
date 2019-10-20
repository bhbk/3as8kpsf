using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using static Bhbk.Lib.DataState.Models.KendoPageState;

namespace Bhbk.Lib.DataState.Tests.AttributeTests
{
    public class KendoPageStateAttributeTests
    {
        [Fact]
        public void Attr_KendoPageState_Fail_Filter()
        {
            var state = new KendoPageState()
            {
                Filter = new KendoPageStateFilters { },
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "field1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_KendoPageState_Fail_Sort()
        {
            var state = new KendoPageState()
            {
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_KendoPageState_Fail_Sort_Skip()
        {
            var state = new KendoPageState()
            {
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "field1", Dir = "asc" }
                },
                Skip = -1,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_KendoPageState_Fail_Sort_Take()
        {
            var state = new KendoPageState()
            {
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "field1", Dir = "asc" }
                },
                Skip = 0,
                Take = 0
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_KendoPageState_Success_Filter()
        {
            var state = new KendoPageState()
            {
                Filter = new KendoPageStateFilters()
                {
                    Logic = "and",
                    Filters = new List<KendoPageStateFilters>()
                    {
                        new KendoPageStateFilters { Field = "string1", Operator = "contains", Value = "x" },
                        new KendoPageStateFilters { Field = "string1", Operator = "startswith", Value = "y" },
                        new KendoPageStateFilters {
                            Logic = "or",
                            Filters = new List<KendoPageStateFilters>()
                            {
                                new KendoPageStateFilters { Field = "int1", Operator = "eq", Value = "1000 "},
                                new KendoPageStateFilters { Field = "int1", Operator = "gt", Value = "1000 "},
                            }
                        }
                    }
                },
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Fact]
        public void Attr_KendoPageState_Success_Sort()
        {
            var state = new KendoPageState()
            {
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "guid1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "guid2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "date1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "date2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "int1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "int2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "decimal1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "decimal2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "bool1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "bool2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Fact]
        public void Attr_KendoPageState_Success_Sort_NoDir()
        {
            var state = new KendoPageState()
            {
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "guid1" },
                    new KendoPageStateSort() { Field = "guid2" },
                    new KendoPageStateSort() { Field = "date1" },
                    new KendoPageStateSort() { Field = "date2" },
                    new KendoPageStateSort() { Field = "int1" },
                    new KendoPageStateSort() { Field = "int2" },
                    new KendoPageStateSort() { Field = "decimal1" },
                    new KendoPageStateSort() { Field = "decimal2" },
                    new KendoPageStateSort() { Field = "bool1" },
                    new KendoPageStateSort() { Field = "bool2" },
                    new KendoPageStateSort() { Field = "string1" },
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
