using System.Web;
using log4net.Core;

namespace Nebula.MvcApplication.Helpers
{
    public class UserNameProvider : IFixingRequired
    {
        private const string NotAvailable = "n/a";

        public override string ToString()
        {
            var userName = NotAvailable;
            var httpContext = HttpContext.Current;

            if (httpContext != null && httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
                userName = httpContext.User.Identity.Name;

            return userName;
        }

        public object GetFixedObject()
        {
            return ToString();
        }
    }
}