using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Product>();

                    // compose filter
                    message.Filter.Compose<Guid>("productId", value =>
                    {
                        query = query.Where(x => x.Id == value);
                    });

                    message.Filter.Compose<string>("categoryId", value =>
                    {
                        query = query.Where(x => x.Category.Id == value);
                    });

                    message.Filter.Compose<Guid>("supplierId", value =>
                    {
                        query = query.Where(x => x.Supplier.Id == value);
                    });

                    // compose order
                    message.Sorter.Compose("productName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                    });

                    message.Sorter.Compose("categoryName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Category.Name)
                            : query.OrderByDescending(x => x.Category.Name);
                    });

                    message.Sorter.Compose("supplierName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Supplier.Name)
                            : query.OrderByDescending(x => x.Supplier.Name);
                    });

                    message.Sorter.Compose("onHandValue", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.OnHand.Value)
                            : query.OrderByDescending(x => x.Inventory.OnHand.Value);
                    });

                    //message.Sorter.Compose("basePriceAmount", direction =>
                    //{
                    //    query = direction == SortDirection.Ascending
                    //        ? query.OrderBy(x => x.Inventory.BasePrice.Amount)
                    //        : query.OrderByDescending(x => x.Inventory.BasePrice.Amount);
                    //});

                    //message.Sorter.Compose("wholesalePriceAmount", direction =>
                    //{
                    //    query = direction == SortDirection.Ascending
                    //        ? query.OrderBy(x => x.Inventory.WholesalePrice.Amount)
                    //        : query.OrderByDescending(x => x.Inventory.WholesalePrice.Amount);
                    //});

                    //message.Sorter.Compose("retailPriceAmount", direction =>
                    //{
                    //    query = direction == SortDirection.Ascending
                    //        ? query.OrderBy(x => x.Inventory.RetailPrice.Amount)
                    //        : query.OrderByDescending(x => x.Inventory.RetailPrice.Amount);
                    //});

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsFuture = query
                        .Select(x => new Dto.ProductReportPageItem()
                        {
                            Id = x.Id,
                            ProductCode = x.Code,
                            ProductName = x.Name,
                            CategoryName = x.Category.Name,
                            SupplierName = x.Supplier.Name,
                            OnHandValue = x.Inventory.OnHand.Value,
                            //BasePriceAmount = x.Inventory.BasePrice.Amount,
                            //WholesalePriceAmount = x.Inventory.WholesalePrice.Amount,
                            //RetailPriceAmount = x.Inventory.RetailPrice.Amount,
                            //TotalBasePriceAmount = x.Inventory.OnHand.Value * x.Inventory.BasePrice.Amount,
                            //TotalWholesalePriceAmount = x.Inventory.OnHand.Value * x.Inventory.WholesalePrice.Amount,
                            //TotalRetailPriceAmount = x.Inventory.OnHand.Value * x.Inventory.RetailPrice.Amount
                        })
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = itemsFuture.ToList()
                    };

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
