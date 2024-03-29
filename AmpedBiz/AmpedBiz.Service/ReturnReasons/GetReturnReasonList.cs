﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Returns;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.ReturnReasons
{
	public class GetReturnReasonList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.ReturnReason>
        {
            public Response() { }

            public Response(List<Dto.ReturnReason> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.Query<ReturnReason>().Cacheable().ToList();
                    var dtos = entites.MapTo(default(List<Dto.ReturnReason>));

                    response = new Response(dtos);

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
