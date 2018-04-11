using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetVoucher
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : Dto.Voucher { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.QueryOver<PurchaseOrder>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Items.First().Product.Inventories).Eager
                        .FutureValue();

                    var entity = query.Value;
                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
