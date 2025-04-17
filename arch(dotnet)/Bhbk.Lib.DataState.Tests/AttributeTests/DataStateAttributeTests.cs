using Bhbk.Lib.DataState.Extensions;
using Bhbk.Lib.DataState.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;
using Bhbk.Lib.DataState.Models;

namespace Bhbk.Lib.DataState.Tests.Attributes
{
    public class DataStateAttributeTests
    {
        [Theory]
        [InlineData(1, null)]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        [InlineData(-1, 1000)]
        public void Attr_DataState_Skip_Take_Fail(int? skip, int? take)
        {
            string field = "field1";
            string dir = "asc";

            IDataState state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field, Dir = dir },
                },
                Skip = skip ?? 0,
                Take = take ?? 0
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Theory]
        [InlineData("field1", "asc", 0, 1000)]
        [InlineData("field1", "desc", 0, 1000)]
        [InlineData("field1", "asc", 1, 1000)]
        [InlineData("field1", "desc", 1, 1000)]
        public void Attr_DataState_Success(string field, string dir, int skip, int take)
        {
            IDataState state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field, Dir = dir },
                },
                Skip = skip,
                Take = take,
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Theory]
        [InlineData("field1", "asc", "field2", "asc", 0, 1000)]
        [InlineData("field1", "asc", "field2", "desc", 0, 1000)]
        [InlineData("field1", "desc", "field2", "asc", 0, 1000)]
        [InlineData("field1", "desc", "field2", "desc", 0, 1000)]
        [InlineData("field1", "asc", "field2", "asc", 1, 25)]
        [InlineData("field1", "asc", "field2", "desc", 1, 25)]
        [InlineData("field1", "desc", "field2", "asc", 1, 25)]
        [InlineData("field1", "desc", "field2", "desc", 1, 25)]
        public void Attr_DataState_SetDefaultSortCollection_Success(
            string field1, string dir1, string field2, string dir2, int skip, int take)
        {
            IDataState state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field1, Dir = dir1 },
                    new PagerStateSort() { Field = field2, Dir = dir2 }
                },

                Skip = skip,
                Take = take,
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Theory]
        [InlineData("field1", "asrc", 0, 1000)]
        [InlineData("field1", "asrc", 1, 1000)]
        [InlineData("field1", "dasc", 0, 1000)]
        [InlineData("field1", "dasc", 1, 1000)]
        public void Attr_DataState_Sort_Validate_Fail(string field, string dir, int skip, int take)
        {
            IDataState state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field, Dir = dir },
                },

                Skip = skip,
                Take = take,
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Theory]
        [InlineData("field1", "asc", 0, 1000)]
        [InlineData("field1", "desc", 0, 1000)]
        [InlineData("field1", "asc", 1, 1000)]
        [InlineData("field1", "desc", 1, 1000)]
        public void Attr_DataState_Validate_Success(string field, string dir, int skip, int take)
        {
            IDataState state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = field, Dir = dir },
                },

                Skip = skip,
                Take = take,
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Theory]
        [InlineData("field1", "asc")]
        [InlineData("field1", "desc")]
        [InlineData("field2", "asc")]
        public void Attr_DataState_SetDefaultSort_AppliesDefault(string field, string dir)
        {
            var state = new PagerState()
            {
                Skip = 1,
                Take = 1000,
            };

            // No sort exists, so default should be applied
            Assert.True(state.SetDefaultSort(field, dir));
            Assert.NotNull(state.Sort);
            Assert.Single(state.Sort);
            Assert.Equal(field, state.Sort.First().Field);
            Assert.Equal(dir, state.Sort.First().Dir);

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }

        [Theory]
        [InlineData("existingField", "asc", "defaultField", "desc")]
        [InlineData("existingField", "desc", "defaultField", "asc")]
        public void Attr_DataState_SetDefaultSort_KeepsExisting(
            string existingField, string existingDir, string defaultField, string defaultDir)
        {
            var state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = existingField, Dir = existingDir }
                },
                Skip = 1,
                Take = 1000,
            };

            // Valid sort exists, so default should NOT be applied
            Assert.False(state.SetDefaultSort(defaultField, defaultDir));
            Assert.Equal(existingField, state.Sort.First().Field);
            Assert.Equal(existingDir, state.Sort.First().Dir);
        }

        [Theory]
        [InlineData("field1", "asc")]
        [InlineData("field1", "desc")]
        public void Attr_DataState_SetDefaultSort_NotAppliedTwice(string field, string dir)
        {
            var state = new PagerState()
            {
                Skip = 1,
                Take = 1000,
            };

            // First call applies default
            Assert.True(state.SetDefaultSort(field, dir));

            // Second call should NOT apply default (sort already exists)
            Assert.False(state.SetDefaultSort(field, dir));
        }

        [Theory]
        [InlineData("field1", "asc", "field2", "desc")]
        [InlineData("field1", "desc", "field2", "asc")]
        public void Attr_DataState_SetDefaultSort_WithCollection(
            string field1, string dir1, string field2, string dir2)
        {
            var state = new PagerState()
            {
                Skip = 1,
                Take = 1000,
            };

            var defaultSort = new List<IDataStateSort>
            {
                new PagerStateSort { Field = field1, Dir = dir1 },
                new PagerStateSort { Field = field2, Dir = dir2 }
            };

            Assert.True(state.SetDefaultSort(defaultSort));
            Assert.Equal(2, state.Sort.Count);
        }
    }
}
