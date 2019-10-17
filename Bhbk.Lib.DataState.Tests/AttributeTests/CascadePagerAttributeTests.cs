using Bhbk.Lib.Cryptography.Entropy;
using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using static Bhbk.Lib.DataState.Models.PageState;

namespace Bhbk.Lib.DataState.Tests.AttributeTests
{
    public class CascadePagerAttributeTests
    {
        [Fact]
        public void Attr_CascadePager_Fail_Fields_Sort()
        {
            var state = new CascadePager()
            {
                Sort = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("field1", AlphaNumeric.CreateString(8))
                },
                Skip = 0,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_CascadePager_Fail_Skip()
        {
            var state = new CascadePager()
            {
                Sort = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("field1", "asc")
                },
                Skip = -1000,
                Take = 1000
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_CascadePager_Fail_Take()
        {
            var state = new CascadePager()
            {
                Sort = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("field1", "asc")
                },
                Skip = 0,
                Take = 0
            };

            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(state, new ValidationContext(state), results, true);
            Assert.False(actual);
        }

        [Fact]
        public void Attr_CascadePager_Success()
        {
            var state = new CascadePager()
            {
                Sort = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("field1", "asc")
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
