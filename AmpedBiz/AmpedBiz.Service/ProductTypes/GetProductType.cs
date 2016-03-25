using MediatR;

namespace AmpedBiz.Service.ProductTypes
{
    public class GetProductType
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

        public class Response : Dto.ProductType
        {
        }

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
