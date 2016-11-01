using AmpedBiz.Service.Host.Auth.Models;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AmpedBiz.Service.Host.Auth.Filters
{

    public class RoleAttribute : Attribute, IAuthorizationFilter
    {
        public bool AllowMultiple { get { return true; } }

        public Role[] Roles { get; set; }

        public RoleAttribute() { }

        public RoleAttribute(Role[] roles)
        {
            this.Roles = roles;
        }
        
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
        }
    }
}