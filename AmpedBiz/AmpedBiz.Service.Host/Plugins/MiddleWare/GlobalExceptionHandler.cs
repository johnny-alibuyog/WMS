using AmpedBiz.Common.Exceptions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace AmpedBiz.Service.Host.Plugins.MiddleWare
{
    public class ErrorMessageResult : IHttpActionResult
    {
        private HttpRequestMessage _request;
        private HttpResponseMessage _httpResponseMessage;


        public ErrorMessageResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
        {
            _request = request;
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }

    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is ValidationException || context.Exception is BusinessException)
            {
                var messge = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = context.Exception.Message
                };
                context.Result = new ErrorMessageResult(context.Request, messge);
            }
            else if (context.Exception is ResourceNotFoundException)
            {
                var messge = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = context.Exception.Message
                };
                context.Result = new ErrorMessageResult(context.Request, messge);
            }
            else if (context.Exception is ResourceAlreadyExistsException)
            {
                var messge = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = context.Exception.Message
                };
                context.Result = new ErrorMessageResult(context.Request, messge);
            }
            else
            {
                base.Handle(context);
            }
        }

        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            this.Handle(context);
            return Task.FromResult(0);
        }

        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
}