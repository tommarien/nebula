using System;
using System.Web;
using System.Web.Security;
using Nebula.Contracts.Registration.Commands;
using Newtonsoft.Json;

namespace Nebula.MvcApplication.Services
{
    public interface IFormsAuthenticationService
    {
        string FormsCookieName { get; }

        FormsAuthenticationTicket Decrypt(string encryptedTicket);

        void SignIn(AuthenticationResult result, bool rememberMe);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public string FormsCookieName
        {
            get { return FormsAuthentication.FormsCookieName; }
        }

        public FormsAuthenticationTicket Decrypt(string encryptedTicket)
        {
            return FormsAuthentication.Decrypt(encryptedTicket);
        }

        public void SignIn(AuthenticationResult result, bool rememberMe)
        {
            string userData = JsonConvert.SerializeObject(result);

            var authTicket = new FormsAuthenticationTicket(
                1,
                result.UserId.ToString(),
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                rememberMe,
                userData,
                FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                        FormsAuthentication.Encrypt(authTicket));

            cookie.Expires = rememberMe ? DateTime.Now.AddYears(1) : authTicket.Expiration;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}