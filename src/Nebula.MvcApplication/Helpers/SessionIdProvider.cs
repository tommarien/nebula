using System.Web;

namespace Nebula.MvcApplication.Helpers
{
    public class SessionIdProvider
    {
        public override string ToString()
        {
            return HttpContext.Current.Session.SessionID;
        }
    }
}