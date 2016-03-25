using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace AmpedBiz.Service.ProductTypes
{
    public class CreateProductType
    {
        public class Request : Dto.ProductType, IRequest<Response> { }

        public class Response : Dto.ProductType { }

        public class Handler : IRequestHandler<Request, Response>
        {
            public Response Handle(Request message)
            {
                return new Response()
                {
                    Id = message.Id,
                    Name = $"Name {message.Id}"
                };
            }
        }
    }
}
