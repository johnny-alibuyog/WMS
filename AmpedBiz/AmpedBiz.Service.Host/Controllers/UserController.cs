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
        public GetUser.Response Process([FromUri]GetUser.Request request)
        {
            return _mediator.Send(request ?? new GetUser.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetUserList.Response Process([FromUri]GetUserList.Request request)
        {
            return _mediator.Send(request ?? new GetUserList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetUserPage.Response Process([FromBody]GetUserPage.Request request)
        {
            return _mediator.Send(request ?? new GetUserPage.Request());
        }

        [HttpGet()]
        [Route("initial")]
        public GetInitialUser.Response Process([FromUri]GetInitialUser.Request request)
        {
            return _mediator.Send(request ?? new GetInitialUser.Request());
        }

        [HttpPost()]
        [Route("login")]
        public Login.Response Process([FromBody]Login.Request request)
        {
            return _mediator.Send(request ?? new Login.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateUser.Response Process([FromBody]CreateUser.Request request)
        {
            return _mediator.Send(request ?? new CreateUser.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateUser.Response Process([FromBody]UpdateUser.Request request)
        {
            return _mediator.Send(request ?? new UpdateUser.Request());
        }
    }
}