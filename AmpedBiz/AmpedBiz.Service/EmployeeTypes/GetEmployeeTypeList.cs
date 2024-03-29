﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.EmployeeTypes
{
    public class GetEmployeeTypeList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.EmployeeType>
        {
            public Response() { }

            public Response(List<Dto.EmployeeType> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.Query<EmployeeType>().ToList();
                    var dtos = entites.MapTo(default(List<Dto.EmployeeType>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
