using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using Nebula.Contracts.Registration.Commands;
using Nebula.MvcApplication.LifecycleEvents;
using Nebula.MvcApplication.Services;
using Newtonsoft.Json;

namespace Nebula.MvcApplication.LifecycleEventHandlers
{
    public class PrincipalProvidingPostAuthenticateRequestEventHandler : IHttpApplicationLifecycleEventHandler<PostAuthenticateRequestEvent>
    {
        private readonly IFormsAuthenticationService formsAuthenticationService;

        public PrincipalProvidingPostAuthenticateRequestEventHandler(IFormsAuthenticationService formsAuthenticationService)
        {
            this.formsAuthenticationService = formsAuthenticationService;
        }

        public void Handle(PostAuthenticateRequestEvent instance)
        {
            HttpContextBase httpContext = instance.HttpContext;

            if (!httpContext.Request.IsAuthenticated) return;

            HttpCookie authCookie = httpContext.Request.Cookies[formsAuthenticationService.FormsCookieName];
            if (authCookie == null) return;

            FormsAuthenticationTicket authTicket = formsAuthenticationService.Decrypt(authCookie.Value);

            var result = JsonConvert.DeserializeObject<AuthenticationResult>(authTicket.UserData);

            var principal = new GenericPrincipal(new GenericIdentity(result.UserName), result.Roles);
            instance.HttpContext.User = Thread.CurrentPrincipal = principal;
        }
    }
}