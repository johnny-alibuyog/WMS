using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Linq;

namespace AmpedBiz.Service.Inventories
{
    public class GetInventoryAdjustmentPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.InventoryAdjustmentPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<InventoryAdjustment>();

                    // compose filters
                    message.Filter.Compose<Guid>("id", value =>
                    {
                        query = query.Where(x => x.Inventory.Id == value);
                    });

                    // compose sort order
                    message.Sorter.Compose("adjustedBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.AdjustedBy.Person.FirstName)
                                .OrderBy(x => x.AdjustedBy.Person.LastName)
                            : query
                                .OrderByDescending(x => x.AdjustedBy.Person.FirstName)
                                .OrderByDescending(x => x.AdjustedBy.Person.LastName);
                    });

                    message.Sorter.Compose("adjustedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.AdjustedOn)
                            : query.OrderByDescending(x => x.AdjustedOn);
                    });

                    message.Sorter.Compose("reason", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Reason.Name)
                            : query.OrderByDescending(x => x.Reason.Name);
                    });

                    message.Sorter.Compose("type", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Type)
                            : query.OrderByDescending(x => x.Type);
                    });

                    message.Sorter.Compose("measure", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Quantity.Value)
                            : query.OrderByDescending(x => x.Quantity.Value);
                    });

                    //query
                    //    .Fetch(x => x.Reason)
                    //    .ToFuture();

                    //query
                    //    .Fetch(x => x.Inventory)
                    //    .ThenFetch(x => x.Product)
                    //    .ThenFetchMany(x => x.UnitOfMeasures)
                    //    .ThenFetch(x => x.UnitOfMeasure);

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    var idsFuture = query
                        .Select(x => x.Id)
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var adjustments = session.QueryOver<InventoryAdjustment>()
                        .WhereRestrictionOn(x => x.Id).IsInG(idsFuture)
                        .Fetch(x => x.Reason).Eager
                        .Fetch(x => x.Inventory).Eager
                        .Fetch(x => x.Inventory.Product).Eager
                        .Fetch(x => x.Inventory.Product.UnitOfMeasures).Eager
                        .Fetch(x => x.Inventory.Product.UnitOfMeasures.First().UnitOfMeasure).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .Future();

                    var Hydrate = new Func<Guid, Dto.InventoryAdjustmentPageItem>((adjustmentId) =>
                    {
                        var adjustment = adjustments.Single(x => x.Id == adjustmentId);

                        var item = new Dto.InventoryAdjustmentPageItem()
                        {
                            Id = adjustment.Inventory.Product.Id,
                            AdjustedBy =
                                adjustment.AdjustedBy.Person.FirstName + " " +
                                adjustment.AdjustedBy.Person.LastName,
                            AdjustedOn = adjustment.AdjustedOn,
                            Reason = adjustment.Reason.Name,
                            Remarks = adjustment.Remarks,
                            Type = adjustment.Type.ToString(),
                            Quantity = adjustment.Quantity
                                .BreakDown(adjustment.Inventory.Product)
                                .InterpretAsString(),
                        };
                        return item;
                    });

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = idsFuture.Select(Hydrate).ToList()
                    };

                    transaction.Commit();

                    this.SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
