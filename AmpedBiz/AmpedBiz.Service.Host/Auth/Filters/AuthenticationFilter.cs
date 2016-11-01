using AmpedBiz.Service.Host.Auth.Models;
using AmpedBiz.Service.Host.Auth.Tokenizers;
using MediatR;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace AmpedBiz.Service.Host.Auth.Filters
{
    public class AuthenticationFilter : IAuthenticationFilter
    {
        private readonly IMediator _mediator;
        private readonly ITokenizer<User> _tokenizer;

        public bool AllowMultiple { get { return true; } }

        public AuthenticationFilter(IMediator mediator, ITokenizer<User> tokenizer)
        {
            this._mediator = mediator;
            this._tokenizer = tokenizer;
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
                return;

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != "Basic")
                return;

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult(request, "Missing credentials");
                return;
            }

            var parameters = authorization.Parameter.Split(' ');
            if (parameters.Count() != 2)
            {
                context.ErrorResult = new AuthenticationFailureResult(request, "Invalid credentials");
                return;
            }

            var token = parameters.Last();
            var user = this._tokenizer.Decode(token);
            if (user == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(request, "Invalid username or password");
            }

            // 6. If the credentials are valid, set principal.
            else
            {
                context.Principal = user;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public AuthenticationFailureResult(HttpRequestMessage request, string reasonPhrase)
        {
            Request = request;
            ReasonPhrase = reasonPhrase;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = Request,
                ReasonPhrase = ReasonPhrase
            };
        }
    }

    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public IHttpActionResult InnerResult { get; private set; }

        public AuthenticationHeaderValue Challenge { get; private set; }

        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Only add one challenge per authentication scheme.
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }

            return response;
        }
    }
}