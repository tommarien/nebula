using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using Nebula.Contracts.Registration;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.LifecycleEvents;

namespace Nebula.MvcApplication.LifecycleEventHandlers
{
    public class RoleProvidingPostAuthenticateRequestEventHandler :
        IHttpApplicationLifecycleEventHandler<PostAuthenticateRequestEvent>
    {
        private readonly IQueryHandler<AccountQuery, Role[]> getRolesQuery;

        public RoleProvidingPostAuthenticateRequestEventHandler(IQueryHandler<AccountQuery, Role[]> getRolesQuery)
        {
            this.getRolesQuery = getRolesQuery;
        }

        public void Handle(PostAuthenticateRequestEvent instance)
        {
            HttpContextBase httpContext = instance.HttpContext;

            if (!httpContext.Request.IsAuthenticated) return;

            HttpCookie authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            Role[] roles = getRolesQuery.Execute(new AccountQuery {UserName = authTicket.Name});
            var user = new GenericPrincipal(instance.HttpContext.User.Identity,
                                            roles.Select(r => r.ToString()).ToArray());

            instance.HttpContext.User = Thread.CurrentPrincipal = user;
        }
    }
}