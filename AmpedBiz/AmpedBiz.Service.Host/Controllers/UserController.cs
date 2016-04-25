using System.Web.Http;
using AmpedBiz.Service.Users;
using MediatR;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetUser.Response Get([FromUri]GetUser.Request request)
        {
            return _mediator.Send(request ?? new GetUser.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetUsers.Response Get([FromUri]GetUsers.Request request)
        {
            return _mediator.Send(request ?? new GetUsers.Request());
        }

        [HttpPost()]
        [Route("pages")]
        public GetUserPages.Response Page([FromBody]GetUserPages.Request request)
        {
            return _mediator.Send(request ?? new GetUserPages.Request());
        }

        [HttpGet()]
        [Route("initial")]
        public GetInitialUser.Response Get([FromUri]GetInitialUser.Request request)
        {
            return _mediator.Send(request ?? new GetInitialUser.Request());
        }

        [HttpPost()]
        [Route("login")]
        public Login.Response Login([FromBody]Login.Request request)
        {
            return _mediator.Send(request ?? new Login.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateUser.Response Create([FromBody]CreateUser.Request request)
        {
            return _mediator.Send(request ?? new CreateUser.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateUser.Response Update([FromBody]UpdateUser.Request request)
        {
            return _mediator.Send(request ?? new UpdateUser.Request());
        }
    }
}