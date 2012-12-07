using System.Web;
using log4net.Core;

namespace Nebula.MvcApplication.Helpers
{
    public class SessionIdProvider : IFixingRequired
    {
        private const string NotAvailable = "n/a";

        public override string ToString()
        {
            var sessionId = NotAvailable;
            var httpContext = HttpContext.Current;

            if (httpContext != null && httpContext.Session != null)
                sessionId = httpContext.Session.SessionID;

            return sessionId;
        }

        public object GetFixedObject()
        {
            return ToString();
        }
    }
}