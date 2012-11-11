using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using Nebula.MvcApplication.LifecycleEvents;

namespace Nebula.MvcApplication.LifecycleEventHandlers
{
    public class CookieAuthenticateRequestEventHandler : IHttpApplicationLifecycleEventHandler<AuthenticateRequestEvent>
    {
        public void Handle(AuthenticateRequestEvent instance)
        {
            HttpRequestBase request = instance.HttpContext.Request;

            if (!request.IsAuthenticated) return;

            HttpCookie authCookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            string[] roles = authTicket.UserData.Split('|');
            var user = new GenericPrincipal(instance.HttpContext.User.Identity, roles);
            instance.HttpContext.User = Thread.CurrentPrincipal = user;
        }
    }
}