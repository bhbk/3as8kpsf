using Bhbk.Lib.Cryptography.Entropy;
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
                Filter = new RecursiveFilterModel { },
                Sort = new List<SortModel>()
                {
                    new SortModel() { Field = "field1", Dir = "asc" }
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
                Sort = new List<SortModel>()
                {
                    new SortModel() { Field = "field1", Dir = AlphaNumeric.CreateString(8) }
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);

            state = new PageState()
            {
                Sort = new List<SortModel>() { },
                Skip = 0,
                Take = 1000
            };

            results = new List<ValidationResult>();
            actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_PageState_Fail_Skip()
        {
            var state = new PageState()
            {
                Sort = new List<SortModel>()
                {
                    new SortModel() { Field = "field1", Dir = "asc" }
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
                Sort = new List<SortModel>()
                {
                    new SortModel() { Field = "field1", Dir = "asc" }
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
                Filter = new RecursiveFilterModel()
                {
                    Logic = "and",
                    Filters = new List<RecursiveFilterModel>()
                    {
                        new RecursiveFilterModel { Field = "string1", Operator = "contains", Value = "x" },
                        new RecursiveFilterModel { Field = "string1", Operator = "startswith", Value = "y" },
                        new RecursiveFilterModel {
                            Logic = "or",
                            Filters = new List<RecursiveFilterModel>()
                            {
                                new RecursiveFilterModel { Field = "int1", Operator = "eq", Value = "1000 "},
                                new RecursiveFilterModel { Field = "int1", Operator = "gt", Value = "1000 "},
                            }
                        }
                    }
                },
                Sort = new List<SortModel>()
                {
                    new SortModel() { Field = "guid1", Dir = "asc" },
                    new SortModel() { Field = "guid2", Dir = "asc" },
                    new SortModel() { Field = "date1", Dir = "asc" },
                    new SortModel() { Field = "date2", Dir = "asc" },
                    new SortModel() { Field = "int1", Dir = "asc" },
                    new SortModel() { Field = "int2", Dir = "asc" },
                    new SortModel() { Field = "decimal1", Dir = "asc" },
                    new SortModel() { Field = "decimal2", Dir = "asc" },
                    new SortModel() { Field = "bool1", Dir = "asc" },
                    new SortModel() { Field = "bool2", Dir = "asc" },
                    new SortModel() { Field = "string1", Dir = "asc" },
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
