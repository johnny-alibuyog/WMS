using System.Collections.Generic;
using MediatR;

namespace AmpedBiz.Service.ProductTypes
{
    public class GetProductTypes
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.ProductType>
        {
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public Response Handle(Request message)
            {
                return new Response()
                {
                    new Dto.ProductType() { Id = "1", Name = "Product 1" },
                    new Dto.ProductType() { Id = "2", Name = "Product 2" },
                    new Dto.ProductType() { Id = "3", Name = "Product 3" },
                    new Dto.ProductType() { Id = "4", Name = "Product 4" },
                    new Dto.ProductType() { Id = "5", Name = "Product 5" },
                };
            }
        }
    }
}
