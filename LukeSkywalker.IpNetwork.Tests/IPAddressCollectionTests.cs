﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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
    /// <summary>
    /// IpNetworkUnitTest test every single method
    /// </summary>
    [TestClass]
    public class IPAddressCollectionTests
    {

        #region Parse

        [TestMethod]
        public void TestAtIndexIPAddress()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/29");
            using (var ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.AreEqual("192.168.1.0", ips[0].ToString(), "0");
                Assert.AreEqual("192.168.1.1", ips[1].ToString(), "1");
                Assert.AreEqual("192.168.1.2", ips[2].ToString(), "2");
                Assert.AreEqual("192.168.1.3", ips[3].ToString(), "3");
                Assert.AreEqual("192.168.1.4", ips[4].ToString(), "4");
                Assert.AreEqual("192.168.1.5", ips[5].ToString(), "5");
                Assert.AreEqual("192.168.1.6", ips[6].ToString(), "6");
                Assert.AreEqual("192.168.1.7", ips[7].ToString(), "7");
            }

        }

        [TestMethod]
        public void TestIterateIPAddress()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/29");
            IPAddress last = null;
            IPAddress fisrt = null;
            int count = 0;
            using (var ips = IPNetwork.ListIPAddress(ipn))
            {
                foreach (var ip in ips)
                {
                    if (fisrt == null) fisrt = ip;
                    last = ip;
                    count++;
                }
                Assert.IsNotNull(last, "last is null");
                Assert.IsNotNull(fisrt, "fisrt is null");
                Assert.AreEqual("192.168.1.0", fisrt.ToString(), "first");
                Assert.AreEqual("192.168.1.7", last.ToString(), "last");
                Assert.AreEqual(8, count, "count");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestOutOfRangeIPAddress()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/29");
            using (var ips = IPNetwork.ListIPAddress(ipn))
            {
                Console.Write("This is out of range : {0} ", ips[8]);
            }

        }

        [TestMethod]
        public void TestCountIPAddress()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/29");
            using (IPAddressCollection ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.AreEqual(8, ips.Count, "Count");
            }
        }

        [TestMethod]
        public void TestReset()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/29");
            using (IPAddressCollection ips = IPNetwork.ListIPAddress(ipn))
            {
                ips.Reset();
            }
        }

        [TestMethod]
        public void TestResetEnumerator()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/29");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn))
            {
                ips.Reset();
                while (ips.MoveNext())
                {
                    Assert.IsNotNull(ips.Current);
                }
                ips.Reset();
                while (ips.MoveNext())
                {
                    Assert.IsNotNull(ips.Current);
                }

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestEnumeratorFailed()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/29");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn))
            {
                ips.Reset();
                while (ips.MoveNext())
                {
                    Assert.IsNotNull(ips.Current);
                }
                Console.WriteLine("This is out of range : {0}", ips.Current);

            }
        }

        [TestMethod]
        public void TestEnumeratorMoveNext()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/29");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());


            }
        }

        [TestMethod]
        public void TestEnumeratorMoveNext2()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/31");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());
                ips.Reset();
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());


            }
        }

        [TestMethod]
        public void TestEnumeratorCurrent()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/31");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsFalse(ips.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestEnumeratorCurrentOor()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/31");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsFalse(ips.MoveNext());
            Console.WriteLine("This is out of range : {0} ", ips.Current);
        }

        [TestMethod]
        public void TestEnumeratorIterate()
        {

            IPNetwork ipn = IPNetwork.Parse("192.168.1.0/31");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            while (ips.MoveNext())
            {
                Assert.IsNotNull(ips.Current);
            }
        }


        #endregion

        #region IPv6


        [TestMethod]
        public void Test_ipv6_AtIndexIPAddress()
        {

            IPNetwork ipn = IPNetwork.Parse("::/125");
            using (var ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.AreEqual("::", ips[0].ToString(), "0");
                Assert.AreEqual("::1", ips[1].ToString(), "1");
                Assert.AreEqual("::2", ips[2].ToString(), "2");
                Assert.AreEqual("::3", ips[3].ToString(), "3");
                Assert.AreEqual("::4", ips[4].ToString(), "4");
                Assert.AreEqual("::5", ips[5].ToString(), "5");
                Assert.AreEqual("::6", ips[6].ToString(), "6");
                Assert.AreEqual("::7", ips[7].ToString(), "7");
            }

        }

        [TestMethod]
        public void Test_ipv6_IterateIPAddress()
        {

            IPNetwork ipn = IPNetwork.Parse("::/125");
            IPAddress last = null;
            IPAddress fisrt = null;
            int count = 0;
            using (var ips = IPNetwork.ListIPAddress(ipn))
            {
                foreach (var ip in ips)
                {
                    if (fisrt == null) fisrt = ip;
                    last = ip;
                    count++;
                }
                Assert.IsNotNull(last, "last is null");
                Assert.IsNotNull(fisrt, "fisrt is null");
                Assert.AreEqual("::", fisrt.ToString(), "first");
                Assert.AreEqual("::7", last.ToString(), "last");
                Assert.AreEqual(8, count, "count");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ipv6_OutOfRangeIPAddress()
        {

            IPNetwork ipn = IPNetwork.Parse("::/125");
            using (var ips = IPNetwork.ListIPAddress(ipn))
            {
                Console.Write("This is out of range : {0} ", ips[8]);
            }

        }

        [TestMethod]
        public void Test_ipv6_CountIPAddress()
        {

            IPNetwork ipn = IPNetwork.Parse("::/125");
            using (IPAddressCollection ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.AreEqual(8, ips.Count, "Count");
            }
        }

        [TestMethod]
        public void Test_ipv6_CountIPAddress2()
        {

            IPNetwork ipn = IPNetwork.Parse("::/0");
            var max = BigInteger.Pow(2, 128);
            using (IPAddressCollection ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.AreEqual(max, ips.Count, "Count");
            }
        }

        [TestMethod]
        public void Test_ipv6_Reset()
        {

            IPNetwork ipn = IPNetwork.Parse("::/125");
            using (IPAddressCollection ips = IPNetwork.ListIPAddress(ipn))
            {
                ips.Reset();
            }
        }

        [TestMethod]
        public void Tes_ipv6_tResetEnumerator()
        {

            IPNetwork ipn = IPNetwork.Parse("::/125");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn))
            {
                ips.Reset();
                while (ips.MoveNext())
                {
                    Assert.IsNotNull(ips.Current);
                }
                ips.Reset();
                while (ips.MoveNext())
                {
                    Assert.IsNotNull(ips.Current);
                }

            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ipv6_EnumeratorFailed()
        {

            IPNetwork ipn = IPNetwork.Parse("::/125");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn))
            {
                ips.Reset();
                while (ips.MoveNext())
                {
                    Assert.IsNotNull(ips.Current);
                }
                Console.WriteLine("This is out of range : {0}", ips.Current);

            }
        }

        [TestMethod]
        public void Test_ipv6_EnumeratorMoveNext()
        {

            IPNetwork ipn = IPNetwork.Parse("::/125");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());


            }
        }

        [TestMethod]
        public void Test_ipv6_EnumeratorMoveNext2()
        {

            IPNetwork ipn = IPNetwork.Parse("::/127");
            using (IEnumerator<IPAddress> ips = IPNetwork.ListIPAddress(ipn))
            {
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());
                ips.Reset();
                Assert.IsTrue(ips.MoveNext());
                Assert.IsTrue(ips.MoveNext());
                Assert.IsFalse(ips.MoveNext());


            }
        }

        [TestMethod]
        public void Test_ipv6_EnumeratorCurrent()
        {

            IPNetwork ipn = IPNetwork.Parse("::/127");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsFalse(ips.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ipv6_EnumeratorCurrentOor()
        {

            IPNetwork ipn = IPNetwork.Parse("::/127");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsNotNull(ips.Current);
            Assert.IsTrue(ips.MoveNext());
            Assert.IsFalse(ips.MoveNext());
            Console.WriteLine("This is out of range : {0} ", ips.Current);
        }

        [TestMethod]
        public void Test_ipv6_EnumeratorIterate()
        {

            IPNetwork ipn = IPNetwork.Parse("::/127");
            IEnumerator ips = IPNetwork.ListIPAddress(ipn);
            while (ips.MoveNext())
            {
                Assert.IsNotNull(ips.Current);
            }
        }

        [TestMethod]
        public void Test_ipv6_DefaultNetmask()
        {
            var ipnetwork = IPNetwork.Parse("::1");
            Assert.AreEqual(64, ipnetwork.Cidr, "Cidr");
        }



        #endregion

    }
}
