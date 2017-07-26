using AmpedBiz.Service.Users;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

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
        public async Task<GetUser.Response> Process([FromUri]GetUser.Request request)
        {
            return await _mediator.Send(request ?? new GetUser.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetUserList.Response> Process([FromUri]GetUserList.Request request)
        {
            return await _mediator.Send(request ?? new GetUserList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetUserPage.Response> Process([FromBody]GetUserPage.Request request)
        {
            return await _mediator.Send(request ?? new GetUserPage.Request());
        }

        [HttpGet()]
        [Route("~/user-lookups")]
        public async Task<GetUserLookup.Response> Process([FromBody]GetUserLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetUserLookup.Request());
        }

        [HttpGet()]
        [Route("initial")]
        public async Task<GetInitialUser.Response> Process([FromUri]GetInitialUser.Request request)
        {
            return await _mediator.Send(request ?? new GetInitialUser.Request());
        }

        [HttpPost()]
        [Route("login")]
        public async Task<Login.Response> Process([FromBody]Login.Request request)
        {
            return await _mediator.Send(request ?? new Login.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreateUser.Response> Process([FromBody]CreateUser.Request request)
        {
            return await _mediator.Send(request ?? new CreateUser.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdateUser.Response> Process([FromBody]UpdateUser.Request request)
        {
            return await _mediator.Send(request ?? new UpdateUser.Request());
        }
    }
}