using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using System;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class GetOrder
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }

            public Request() : this(default(Guid)) { }

            public Request(Guid id) => this.Id = id;
        }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                if (message.Id == Guid.Empty)
                {
                    var entity = new Order();
                    entity.MapTo(response);

                    return response;
                }

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.QueryOver<Order>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Branch).Eager
                        .Fetch(x => x.Customer).Eager
                        .Fetch(x => x.Pricing).Eager
                        .Fetch(x => x.PaymentType).Eager
                        .Fetch(x => x.Shipper).Eager
                        .Fetch(x => x.Tax).Eager
                        .Fetch(x => x.ShippingFee).Eager
                        .Fetch(x => x.Discount).Eager
                        .Fetch(x => x.SubTotal).Eager
                        .Fetch(x => x.Total).Eager
                        .Fetch(x => x.CreatedBy).Eager
                        .Fetch(x => x.OrderedBy).Eager
                        .Fetch(x => x.RoutedBy).Eager
                        .Fetch(x => x.StagedBy).Eager
                        .Fetch(x => x.InvoicedBy).Eager
                        .Fetch(x => x.PaidTo).Eager
                        .Fetch(x => x.RoutedBy).Eager
                        .Fetch(x => x.CompletedBy).Eager
                        .Fetch(x => x.CancelledBy).Eager
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Returns).Eager
                        .Fetch(x => x.Returns.First().Product).Eager
                        .Fetch(x => x.Payments).Eager
                        .Fetch(x => x.Payments.First().PaidTo).Eager
                        .Fetch(x => x.Payments.First().PaymentType).Eager
                        .SingleOrDefault();

                    entity.EnsureExistence($"Order with id {message.Id} does not exists.");
                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}