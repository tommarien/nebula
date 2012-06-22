using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using Nebula.Contracts.Registration;

namespace Nebula.MvcApplication.Services
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie, Role[] roles);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie, Role[] roles)
        {
            string formattedRoles = string.Empty;
            if (roles.Any())
            {
                formattedRoles = String.Join("|", roles);
            }

            HttpContext context = HttpContext.Current;

            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: userName,
                issueDate: DateTime.UtcNow,
                expiration: DateTime.UtcNow.Add(FormsAuthentication.Timeout),
                isPersistent: createPersistentCookie,
                userData: formattedRoles
                );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var formsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            if (createPersistentCookie) formsCookie.Expires = ticket.Expiration;

            context.Response.Cookies.Add(formsCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}