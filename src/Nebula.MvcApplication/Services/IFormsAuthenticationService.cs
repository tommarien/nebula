using System.Web.Security;

namespace Nebula.MvcApplication.Services
{
    public interface IFormsAuthenticationService
    {
        string FormsCookieName { get; }

        FormsAuthenticationTicket Decrypt(string encryptedTicket);

        void SignIn(string userName, bool createPersistentCookie);
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

        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}