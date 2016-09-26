using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductPurchasePage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductPurchasePageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var productId = message.Filter["id"].ToString();

                    // compose query
                    var query = session.Query<PurchaseOrderItem>()
                        .Where(x => x.Product.Id.ToString() == productId)
                        .Select(x => new Dto.ProductPurchasePageItem()
                        {
                            Id = x.PurchaseOrder.Id,
                            PurchaseOrderNumber = x.PurchaseOrder.PurchaseOrderNumber,
                            CreatedOn = x.PurchaseOrder.CreatedOn,
                            Status = x.PurchaseOrder.Status.ToString(),
                            SupplierName = x.PurchaseOrder.Supplier.Name,
                            UnitCostAmount = x.UnitCost.Amount,
                            QuantityValue = x.Quantity.Value,
                            ReceivedValue = session
                                .Query<PurchaseOrderReceipt>()
                                .Where(o => 
                                    o.Product == x.Product &&
                                    o.PurchaseOrder == x.PurchaseOrder
                                )
                                .Sum(o => o.Quantity.Value)
                        });

                    // compose sort order
                    message.Sorter.Compose("purchaseOrderNumber", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PurchaseOrderNumber)
                            : query.OrderByDescending(x => x.PurchaseOrderNumber);
                    });

                    message.Sorter.Compose("createdOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.CreatedOn)
                            : query.OrderByDescending(x => x.CreatedOn);
                    });

                    message.Sorter.Compose("status", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Status)
                            : query.OrderByDescending(x => x.Status);
                    });

                    message.Sorter.Compose("supplier", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.SupplierName)
                            : query.OrderByDescending(x => x.SupplierName);
                    });

                    message.Sorter.Compose("unitCost", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.UnitCostAmount)
                            : query.OrderByDescending(x => x.UnitCostAmount);
                    });

                    message.Sorter.Compose("quantity", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.QuantityValue)
                            : query.OrderByDescending(x => x.QuantityValue);
                    });

                    message.Sorter.Compose("received", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ReceivedValue)
                            : query.OrderByDescending(x => x.ReceivedValue);
                    });

                    var itemsFuture = query
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = itemsFuture.ToList()
                    };
                }

                return response;
            }
        }
    }
}
