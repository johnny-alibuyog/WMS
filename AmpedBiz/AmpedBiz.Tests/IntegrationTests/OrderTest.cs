using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using NUnit.Framework;

namespace AmpedBiz.Tests.IntegrationTests
{
    [TestFixture]
    public class OrderTest
    {
        [Test]
        public void Test()
        {
            var order = new Order();

            for (int i = 0; i < 100; i++)
            {
                //order.AddOrderItem(new OrderItem() { ExtendedPrice = new Money(10) });
            }
        }
    }
}
