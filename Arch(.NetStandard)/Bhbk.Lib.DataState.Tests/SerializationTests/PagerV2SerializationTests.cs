using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using Xunit;

namespace Bhbk.Lib.DataState.Tests.SerializationTests
{
    public class PagerV2SerializationTests
    {
        [Fact]
        public void Serialize_PagerV2_Success()
        {
            var state = new PagerV2()
            {
                Sort = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("field1", "asc")
                },
                Skip = 1,
                Take = 1000,
            };

            var valid = state.GetType();
            Assert.True(valid.IsSerializable);
        }
    }
}
