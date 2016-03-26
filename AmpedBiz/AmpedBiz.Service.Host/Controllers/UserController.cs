using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        [HttpGet()]
        [Route("{request?}")]
        public string Get(string request = null)
        {
            return "Users!";
        }
    }
}