using System.Web;

namespace Nebula.MvcApplication.Helpers
{
    public class SessionIdProvider
    {
        public override string ToString()
        {
            var session = HttpContext.Current.Session;
            return session == null ? "n/a" : session.SessionID;
        }
    }
}