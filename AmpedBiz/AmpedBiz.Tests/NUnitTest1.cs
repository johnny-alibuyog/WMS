﻿using System;
using NUnit.Framework;
using System.Linq;
using AmpedBiz.Data.Context;

namespace AmpedBiz.Tests
{
    [TestFixture]
    public class NUnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var xxx1 = new int[] { };
            var xxx12 = xxx1.FirstOrDefault();
        }

        [Test]
        public void Test()
        {
            var x = (TenantId)"ampbiz";
            var y = (string)x;
        }
    }
}