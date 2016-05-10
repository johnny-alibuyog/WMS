using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Branches
{
    public class GetBrancheList
    {
        public class Request : IRequest<Response>
        {
            public string[] Ids { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }

        public class Response : List<Dto.Branch>
        {
            public Response() { }

            public Response(List<Dto.Branch> items) : base(items) { }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISessionFactory _sessionFactory;

            public Handler(ISessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Response Handle(Request message)
            {
                var response = default(Response);

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    //var query = session.Query<Entity.Branch>();

                    //if (message.Ids != null && message.Ids.Count() > 0)
                    //    query = query.Where(x => message.Ids.Contains(x.Id));

                    //if (message.Name != null)
                    //    query = query.Where(x => x.Name.StartsWith(message.Name));

                    //if (message.Description != null)
                    //    query = query.Where(x => x.Name.StartsWith(message.Description));

                    //var selectQuery = query
                    //    .Select(x => new Dto.Branch()
                    //    {
                    //        Id = x.Id,
                    //        Name = x.Name,
                    //        Description = x.Description
                    //    });

                    //switch (message.Sort.Field)
                    //{
                    //    case "Name":
                    //        selectQuery = message.Sort.Direction == SortDirection.Ascending
                    //            ? selectQuery.OrderBy(x => x.Name)
                    //            : selectQuery.OrderByDescending(x => x.Name);
                    //        break;
                    //    case "Description":
                    //        selectQuery = message.Sort.Direction == SortDirection.Ascending
                    //            ? selectQuery.OrderBy(x => x.Description)
                    //            : selectQuery.OrderByDescending(x => x.Description);
                    //        break;
                    //    default:
                    //        selectQuery = message.Sort.Direction == SortDirection.Ascending
                    //            ? selectQuery.OrderBy(x => x.Name)
                    //            : selectQuery.OrderByDescending(x => x.Name);
                    //        break;
                    //}

                    //var itemsFuture = selectQuery
                    //    .Skip(message.Page.SkipCount)
                    //    .Take(message.Page.Size)
                    //    .ToFuture();

                    //var countFuture = selectQuery
                    //    .ToFutureValue(x => x.Count());

                    //response = new Response()
                    //{
                    //    Page = new PageResponse<Dto.Branch>()
                    //    {
                    //        Count = countFuture.Value,
                    //        Items = itemsFuture.ToList()
                    //    }
                    //};

                    var entites = session.Query<Entity.Branch>()
                        .Select(x => new Dto.Branch()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .ToList();

                    response = new Response(entites);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
