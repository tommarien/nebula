using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using NHibernate;
using Nebula.Domain.Registration;
using Nebula.MvcApplication.LifecycleEvents;
using Nebula.MvcApplication.Services;
using NHibernate.Linq;
using Nebula.Data.Registration;

namespace Nebula.MvcApplication.LifecycleEventHandlers
{
    public class PrincipalProvidingAuthenticateRequestEventHandler : IHttpApplicationLifecycleEventHandler<AuthenticateRequestEvent>
    {
        private readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly ISession session;

        public PrincipalProvidingAuthenticateRequestEventHandler(IFormsAuthenticationService formsAuthenticationService, ISession session)
        {
            this.formsAuthenticationService = formsAuthenticationService;
            this.session = session;
        }

        public void Handle(AuthenticateRequestEvent instance)
        {
            HttpContextBase httpContext = instance.HttpContext;

            if (!httpContext.Request.IsAuthenticated) return;
            if (httpContext.User is GenericPrincipal) return;

            HttpCookie authCookie = httpContext.Request.Cookies[formsAuthenticationService.FormsCookieName];
            if (authCookie == null) return;

            FormsAuthenticationTicket authTicket = formsAuthenticationService.Decrypt(authCookie.Value);

            var account = session.Query<Account>()
                                 .WithUserName(authTicket.Name)
                                 .FetchMany(x => x.Roles)
                                 .SingleOrDefault();

            if (account == null)
                return;

            var principal = new GenericPrincipal(new GenericIdentity(account.UserName), account.Roles.Select(x => x.ToString()).ToArray());
            instance.HttpContext.User = Thread.CurrentPrincipal = principal;
        }
    }
}