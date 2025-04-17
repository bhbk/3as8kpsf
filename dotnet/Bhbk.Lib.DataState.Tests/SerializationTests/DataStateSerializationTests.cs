using Bhbk.Lib.DataState.Interfaces;
using System.Collections.Generic;
using Xunit;
using Bhbk.Lib.DataState.Models;

namespace Bhbk.Lib.DataState.Tests.SerializationTests
{
    public class DataStateSerializationTests
    {
        [Fact]
        public void Serialize_DataState_Success()
        {
            var state = new PagerState()
            {
                Sort = new List<IDataStateSort>()
                {
                    new PagerStateSort() { Field = "field1", Dir = "asc" }
                },
                Skip = 1,
                Take = 1000,
            };

            var valid = state.GetType();
            Assert.True(valid.IsSerializable);
        }
    }
}
