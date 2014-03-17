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

        void SignIn(AuthenticationResult result, bool createPersistentCookie);
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

        public void SignIn(AuthenticationResult result, bool createPersistentCookie)
        {
            string userData = JsonConvert.SerializeObject(result);

            var authTicket = new FormsAuthenticationTicket(
                1,
                result.UserId.ToString(),
                DateTime.Now,
                DateTime.Now.AddMinutes(30), // expiry
                createPersistentCookie, //do not remember
                userData,
                "/");

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                        FormsAuthentication.Encrypt(authTicket));

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}