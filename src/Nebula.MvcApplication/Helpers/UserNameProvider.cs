using System.Web;

namespace Nebula.MvcApplication.Helpers
{
    public class UserNameProvider
    {
        private const string Anonymous = "Anonymous";

        public override string ToString()
        {
            var userName = HttpContext.Current.User.Identity.Name;
            return string.IsNullOrEmpty(userName) ? Anonymous : userName;
        }
    }
}