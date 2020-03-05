using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bhbk.Lib.DataState.Tests.AttributeTests
{
    public class DataStateV1AttributeTests
    {
        [Fact]
        public void Attr_DataStateV1_Fail_Filter()
        {
            var state = new DataStateV1()
            {
                Filter = new DataStateV1Filter { },
                Sort = new List<IDataStateSort>()
                {
                    new DataStateV1Sort() { Field = "field1", Dir = "asc" }
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_DataStateV1_Fail_Sort()
        {
            var state = new DataStateV1()
            {
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_DataStateV1_Fail_Sort_Skip()
        {
            var state = new DataStateV1()
            {
                Sort = new List<IDataStateSort>()
                {
                    new DataStateV1Sort() { Field = "field1", Dir = "asc" }
                },
                Skip = -1,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_DataStateV1_Fail_Sort_Take()
        {
            var state = new DataStateV1()
            {
                Sort = new List<IDataStateSort>()
                {
                    new DataStateV1Sort() { Field = "field1", Dir = "asc" }
                },
                Skip = 0,
                Take = 0
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_DataStateV1_Success_Filter()
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

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Fact]
        public void Attr_DataStateV1_Success_Sort()
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

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Fact]
        public void Attr_DataStateV1_Success_Sort_NoDir()
        {
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

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }
    }
}
