using System.Web;

namespace Nebula.MvcApplication.Helpers
{
    public class UserNameProvider
    {
        private const string NotAvailable = "n/a";

        public override string ToString()
        {
            var userName = HttpContext.Current.User.Identity.Name;
            return string.IsNullOrEmpty(userName) ? NotAvailable : userName;
        }
    }
}