using System.Web;

namespace Nebula.MvcApplication.Helpers
{
    public class SessionIdProvider
    {
        private const string NotAvailable = "n/a";

        public override string ToString()
        {
            var httpcontext = HttpContext.Current;
            if (httpcontext == null) return NotAvailable;
            
            var session = httpcontext.Session;
            return session == null ? NotAvailable : session.SessionID;
        }
    }
}