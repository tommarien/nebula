using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Nebula.MvcApplication.Helpers
{
    public static class MenuItemHelper
    {
        public static MvcHtmlString MenuItem(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            string currentControllerName = (string) helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string) helper.ViewContext.RouteData.Values["action"];

            var sb = new StringBuilder();

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) &&
                currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
                sb.Append("<li class=\"selected\">");
            else
                sb.Append("<li>");

            sb.Append(helper.ActionLink(linkText, actionName, controllerName));
            sb.Append("</li>");
            return new MvcHtmlString(sb.ToString());
        }
    }
}