using AmpedBiz.Service.Customers;
using AmpedBiz.Service.Users;
using AmpedBiz.Tests.Bootstrap;
using Autofac;
using MediatR;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AmpedBiz.Tests.IntegrationTests
{
    [TestFixture]
    public class ReportFeatureTests
    {
        private IMediator _mediator = Ioc.Container.Resolve<IMediator>();

        [Test]
        public async Task GetCustomerSalesReportPage_WhenInvoked_ShouldNotThrowExeption()
        {
            var request = new GetCustomerSalesReportPage.Request();

            var response = await _mediator.Send(request);

            Console.WriteLine(response);
        }

        [Test]
        public async Task GetUserSalesReportPage_WhenInvoked_ShouldNotThrowExeption()
        {
            var request = new GetUserSalesReportPage.Request();

            var response = await _mediator.Send(request);

            Console.WriteLine(response);
        }
    }
}
