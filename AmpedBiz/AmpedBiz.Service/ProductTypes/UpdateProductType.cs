using MediatR;

namespace AmpedBiz.Service.ProductTypes
{
    public class UpdateProductType
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
