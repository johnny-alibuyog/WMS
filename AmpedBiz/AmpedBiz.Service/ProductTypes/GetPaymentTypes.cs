using System.Collections.Generic;
using MediatR;

namespace AmpedBiz.Service.ProductTypes
{
    public class GetPaymentTypes
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.PaymentType>
        {
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public Response Handle(Request message)
            {
                return new Response()
                {
                    new Dto.PaymentType() { Id = "1", Name = "Product 1" },
                    new Dto.PaymentType() { Id = "2", Name = "Product 2" },
                    new Dto.PaymentType() { Id = "3", Name = "Product 3" },
                    new Dto.PaymentType() { Id = "4", Name = "Product 4" },
                    new Dto.PaymentType() { Id = "5", Name = "Product 5" },
                };
            }
        }
    }
}
