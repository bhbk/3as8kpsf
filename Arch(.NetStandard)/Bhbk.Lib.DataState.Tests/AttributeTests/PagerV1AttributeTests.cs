using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bhbk.Lib.DataState.Tests.AttributeTests
{
    public class PagerV1AttributeTests
    {
        [Fact]
        public void Attr_PagerV1_Fail_Sort()
        {
            var state = new PagerV1()
            {
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_PagerV1_Fail_Sort_Skip()
        {
            var state = new PagerV1()
            {
                OrderBy = "field1",
                Order = "asc",
                Skip = -1,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_PagerV1_Fail_Sort_Take()
        {
            var state = new PagerV1()
            {
                OrderBy = "field1",
                Order = "asc",
                Skip = 0,
                Take = 0
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Attr_PagerV1_Success_Sort()
        {
            var state = new PagerV1()
            {
                OrderBy = "field1",
                Order = "asc",
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.True(valid);
        }
    }
}
