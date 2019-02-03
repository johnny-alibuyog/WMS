using AmpedBiz.Service.Users;
using MediatR;
using System;
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
        public async Task<GetUserLookup.Response> Process([FromUri]GetUserLookup.Request request)
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

        [HttpGet()]
        [Route("{id}/address")]
        public async Task<GetUserAddress.Response> Process([FromUri]GetUserAddress.Request request)
        {
            return await _mediator.Send(request ?? new GetUserAddress.Request());
        }

        [HttpGet()]
        [Route("{id}/info")]
        public async Task<GetUserInfo.Response> Process([FromUri]GetUserInfo.Request request)
        {
            return await _mediator.Send(request ?? new GetUserInfo.Request());
        }

        [HttpPut()]
        [Route("{id}/address")]
        public async Task<UpdateUserAddress.Response> Process([FromUri]Guid id, [FromBody]UpdateUserAddress.Request request)
        {
            return await _mediator.Send(request ?? new UpdateUserAddress.Request());
        }

        [HttpPut()]
        [Route("{id}/info")]
        public async Task<UpdateUserInfo.Response> Process([FromUri]Guid id, [FromBody]UpdateUserInfo.Request request)
        {
            return await _mediator.Send(request ?? new UpdateUserInfo.Request());
        }

        [HttpPut()]
        [Route("{id}/password")]
        public async Task<UpdateUserPassword.Response> Process([FromUri]Guid id, [FromBody]UpdateUserPassword.Request request)
        {
            return await _mediator.Send(request ?? new UpdateUserPassword.Request());
        }

		[HttpPost()]
		[Route("{id}/reset-password")]
		public async Task<ResetUserPassword.Response> Process([FromUri]Guid id, [FromBody]ResetUserPassword.Request request)
		{
			return await _mediator.Send(request ?? new ResetUserPassword.Request());
		}
	}
}