using System;
using NUnit.Framework;
using System.Linq;

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
    }
}