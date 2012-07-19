using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Nebula.MvcApplication.Helpers
{
    public static class HtmlHelperEx
    {
        public static MvcForm BeginForm(this HtmlHelper html, object htmlAttributes)
        {
            var controllerName = html.ViewContext.RouteData.GetRequiredString("controller");
            var actionName = html.ViewContext.RouteData.GetRequiredString("action");
            return html.BeginForm(actionName, controllerName, FormMethod.Post, htmlAttributes);
        }

        public static MvcHtmlString BootstrapCheckboxFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression,
                                                                 object htmlAttributes = null)
        {
            var checkbox = html.CheckBoxFor(expression, htmlAttributes);

            var labelbuilder = LabelHelper(html, ModelMetadata.FromLambdaExpression(expression, html.ViewData), ExpressionHelper.GetExpressionText(expression),
                                           new { @class = "checkbox" });
            if (labelbuilder == null) return MvcHtmlString.Empty;

            labelbuilder.InnerHtml = checkbox + labelbuilder.InnerHtml;
            return new MvcHtmlString(labelbuilder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return html.LabelFor(expression, null, htmlAttributes);
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText,
                                                             object htmlAttributes)
        {
            var builder = LabelHelper(html, ModelMetadata.FromLambdaExpression(expression, html.ViewData), ExpressionHelper.GetExpressionText(expression),
                                      htmlAttributes, labelText);
            return builder == null ? MvcHtmlString.Empty : new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }

        public static TagBuilder LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, object htmlAttributes, string labelText = null)
        {
            string innerText = labelText ?? metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (string.IsNullOrEmpty(innerText)) return null;

            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.SetInnerText(innerText);
            return tagBuilder;
        }

        public static MvcHtmlString TrackingListItem(this HtmlHelper html, string linkText, string actionName, string controllerName, string cssClass = "active", string linkItemClass = null)
        {
            var currentControllerName = html.ViewContext.RouteData.GetRequiredString("controller");
            var currentActionName = html.ViewContext.RouteData.GetRequiredString("action");

            var builder = new TagBuilder("li");

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) &&
                currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
                builder.AddCssClass(cssClass);

            var url = UrlHelper.GenerateUrl(null, actionName, controllerName, html.ViewContext.RouteData.Values, html.RouteCollection,
                                            html.ViewContext.RequestContext, true);

            var linkbuilder = new TagBuilder("a")
            {
                InnerHtml = !string.IsNullOrEmpty(linkText) ? HttpUtility.HtmlEncode(linkText) : string.Empty
            };
            linkbuilder.InnerHtml += " <span class=\"" + linkItemClass + "\"></span>";

            linkbuilder.MergeAttribute("href", url);

            builder.InnerHtml = linkbuilder.ToString(TagRenderMode.Normal);

            return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString BootstrapValidationSummary(this HtmlHelper helper,
                                                               bool excludePropertyErrors,
                                                               string message)
        {
            if (helper.ViewData.ModelState.Values.All(v => v.Errors.Count == 0)) return new MvcHtmlString(string.Empty);

            string errorsList = "<ul>";
            foreach (var error in helper.ViewData.ModelState.Values.Where(v => v.Errors.Count > 0))
            {
                errorsList += string.Format("<li>{0}</li>", error.Errors.First().ErrorMessage);
            }
            errorsList += "</ul>";
            return
                new MvcHtmlString(
                    string.Format(
                        "<div class=\"alert alert-block\"><a href=\"#\" data-dismiss=\"alert\" class=\"close\"><span class=\"icon-remove\"></span></a><h4 class=\"alert-heading\">{0}</h4>{1}</div>",
                        message, errorsList));
        }
    }
}