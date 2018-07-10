using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

/*
 * Derivative work based on https://github.com/lduchosal/ipnetwork which was being distributed 
 * under the MIT license when used to seed this portion of the solution.
 * 
 * Adding the 2.x family of this code to the solution via nuget package would be preferred over
 * adding directly in. The current nuget solution requires .Net Core which requires a newer
 * version of Visual Studio.
 */

namespace LukeSkywalker.IPNetwork.Tests
{
    [TestClass]
    public class BigIntegerBitWiseTests
    {

        [TestMethod]
        public void Test1()
        {

            byte[] bytes = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x00 };
            BigInteger reverseme = new BigInteger(bytes);
            BigInteger reversed = reverseme.PositiveReverse(4);

            Assert.AreEqual(0, (int)reversed);


        }

        [TestMethod]
        public void Test2()
        {

            byte[] bytes = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x00 };
            BigInteger reverseme = new BigInteger(bytes);
            BigInteger reversed = reverseme.PositiveReverse(8);
            var result = reversed.ToByteArray();

            Assert.AreEqual(0x0, result[0]);
            Assert.AreEqual(0x0, result[1]);
            Assert.AreEqual(0x0, result[2]);
            Assert.AreEqual(0x0, result[3]);
            Assert.AreEqual(0xFF, result[4]);
            Assert.AreEqual(0xFF, result[5]);
            Assert.AreEqual(0xFF, result[6]);
            Assert.AreEqual(0xFF, result[7]);


        }
    }
}
