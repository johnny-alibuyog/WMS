using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductPurchasePage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductPurchasePageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var productId = message.Filter["id"].ToString();

                    // compose query
                    var query = session.Query<PurchaseOrderItem>();

                    // compose filters
                    message.Filter.Compose<Guid>("id", value =>
                    {
                        query = query.Where(x => x.Product.Id == value);
                    });

                    // compose sort order
                    message.Sorter.Compose("purchaseOrderNumber", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PurchaseOrder.PurchaseOrderNumber)
                            : query.OrderByDescending(x => x.PurchaseOrder.PurchaseOrderNumber);
                    });

                    message.Sorter.Compose("createdOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PurchaseOrder.CreatedOn)
                            : query.OrderByDescending(x => x.PurchaseOrder.CreatedOn);
                    });

                    message.Sorter.Compose("status", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PurchaseOrder.Status)
                            : query.OrderByDescending(x => x.PurchaseOrder.Status);
                    });

                    message.Sorter.Compose("supplier", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PurchaseOrder.Supplier.Name)
                            : query.OrderByDescending(x => x.PurchaseOrder.Supplier.Name);
                    });

                    message.Sorter.Compose("unitCost", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.UnitCost.Amount)
                            : query.OrderByDescending(x => x.UnitCost.Amount);
                    });

                    message.Sorter.Compose("quantityUnit", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Quantity.Unit.Name)
                            : query.OrderByDescending(x => x.Quantity.Unit.Name);
                    });

                    message.Sorter.Compose("quantityValue", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Quantity.Value)
                            : query.OrderByDescending(x => x.Quantity.Value);
                    });

                    //message.Sorter.Compose("received", direction =>
                    //{
                    //    query = direction == SortDirection.Ascending
                    //        ? query.OrderBy(x => x.ReceivedValue)
                    //        : query.OrderByDescending(x => x.ReceivedValue);
                    //});

                    var itemsFuture = query
                        .Select(x => new Dto.ProductPurchasePageItem()
                        {
                            Id = x.PurchaseOrder.Id,
                            PurchaseOrderNumber = x.PurchaseOrder.PurchaseOrderNumber,
                            CreatedOn = x.PurchaseOrder.CreatedOn,
                            Status = x.PurchaseOrder.Status.ToString(),
                            SupplierName = x.PurchaseOrder.Supplier.Name,
                            UnitCostAmount = x.UnitCost.Amount,
                            ReceivedValue = session
                                .Query<PurchaseOrderReceipt>()
                                .Where(o =>
                                    o.Product == x.Product &&
                                    o.PurchaseOrder == x.PurchaseOrder
                                )
                                .Sum(o => o.Quantity.Value),
                            Quantity = new Dto.Measure(
                                x.Quantity.Value,
                                new Dto.UnitOfMeasure(
                                    x.Quantity.Unit.Id,
                                    x.Quantity.Unit.Name
                                )
                            )
                        })
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
