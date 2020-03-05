using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using Xunit;

namespace Bhbk.Lib.DataState.Tests.SerializationTests
{
    public class DataStateV1SerializationTests
    {
        [Fact]
        public void Serialize_DataStateV1_Success()
        {
            var state = new DataStateV1()
            {
                Sort = new List<IDataStateSort>()
                {
                    new DataStateV1Sort() { Field = "field1", Dir = "asc" }
                },
                Skip = 1,
                Take = 1000,
            };

            var valid = state.GetType();
            Assert.True(valid.IsSerializable);
        }
    }
}
