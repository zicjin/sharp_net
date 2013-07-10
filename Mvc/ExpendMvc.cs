using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Microsoft.Web.Mvc;
using System.Reflection;

namespace sharp_net.Mvc {

    public static class ExpendMvc {

        public static string ActionImageLink(this HtmlHelper helper,
            string url, string altText, string defaultUrl, string defaultAltText,
            object imageHtmlAttributes, string actionName, string controllerName,
            object routeValues, object linkHtmlAttributes) {
            string image = helper.Image(url, altText, defaultUrl, defaultAltText, imageHtmlAttributes).ToString();
            MvcHtmlString link = helper.ActionLink("[replaceme]",
                                                   actionName,
                                                   controllerName,
                                                   routeValues,
                                                   linkHtmlAttributes);
            return link.ToString().Replace("[replaceme]", image);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper,
            string linkText, string defaultLinkText, string actionName,
            string controllerName, object routeValues, object htmlAttributes) {
            string rLinkText = string.IsNullOrEmpty(linkText) ? defaultLinkText : linkText;
            if (string.IsNullOrEmpty(rLinkText)) {
                rLinkText = " ";
            }
            return htmlHelper.ActionLink(rLinkText,
                                         actionName,
                                         controllerName,
                                         routeValues,
                                         htmlAttributes);
        }

        public static MvcHtmlString ImageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes) {
            return ImageFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ImageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes) {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            htmlAttributes.Add("name", expressionText);
            return Image(htmlHelper, Convert.ToString(metadata.Model), "", htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper helper,
                                    string url,
                                    string altText,
                                    object htmlAttributes) {
            return Image(helper, url, altText, string.Empty, string.Empty, htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper helper,
                                    string url,
                                    string altText,
                                    string defaultUrl,
                                    string defaultAltText,
                                    object htmlAttributes) {
            return ImageHelper(
                                url,
                                altText,
                                defaultUrl,
                                defaultAltText,
                                htmlAttributes);
        }

        private static MvcHtmlString ImageHelper(
                                string url,
                                string altText,
                                string defaultUrl,
                                string defaultAltText,
                                object htmlAttributes) {
            var builder = new TagBuilder("image");
            builder.Attributes.Add("src", !string.IsNullOrEmpty(url) ? url : defaultUrl);
            builder.Attributes.Add("alt", !string.IsNullOrEmpty(altText) ? altText : defaultAltText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static IList<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items,
            Func<T, string> getText, Func<T, string> getValue, string selectValue) {
            IList<SelectListItem> list = items.OrderBy(i => getText(i))
                .Select(i => new SelectListItem {
                    Selected = (getValue(i) == selectValue),
                    Text = getText(i),
                    Value = getValue(i)
                }).ToList();
            return list;
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj) {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObj);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>(this TEnum enumObj) {
            return from TEnum e in Enum.GetValues(typeof(TEnum))
                   select new SelectListItem() {
                       //Selected = (Convert.ToInt32(e) == Convert.ToInt32(enumObj)),
                       //使用DropDownListFor的话是不需要指定Selected的
                       Text = e.ToString(),
                       Value = Convert.ToInt32(e).ToString()
                   };
        }

        public static IEnumerable<SelectListItem> ToSelectListItemsAttachData<TEnum>(this TEnum enumObj, object attachEnum) {
            return from TEnum e in Enum.GetValues(typeof(TEnum))
                   select new SelectListItem() {
                       Selected = (Convert.ToInt32(e) == Convert.ToInt32(enumObj)),
                       Text = e.GetAttachedDataFromObj<string>(attachEnum),
                       Value = Convert.ToInt32(e).ToString()
                   };
        }

        public static MvcHtmlString ActionLinkWcls<T>(this HtmlHelper helper, Expression<Action<T>> action, string linkText) where T : Controller {
            return helper.ActionLink<T>(action, linkText, new { @class = "lk mr10" });
        }

        public static MvcHtmlString ActionLinkWcls<T>(this HtmlHelper helper, Expression<Action<T>> action, string linkText, object htmlAttributes) where T : Controller {
            RouteValueDictionary routeValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);
            RouteValueDictionary htmlAttrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttrs["class"] == null) {
                htmlAttrs.Add("class", "lk mr10");
            } else {
                htmlAttrs["class"] += " lk mr10";
            }
            return helper.ActionLink(linkText, null, null, routeValues, htmlAttrs);
        }

        public static MvcHtmlString ActionLinkWico<T>(this HtmlHelper helper, Expression<Action<T>> action, string linkText, string icoClass)
            where T : Controller {
            return helper.ActionLink<T>(action, linkText, new { @class = "ico[" + icoClass + "]" });
        }

        public static MvcHtmlString ActionLinkWico<T>(this HtmlHelper helper, Expression<Action<T>> action, string linkText, string icoClass, object htmlAttributes)
            where T : Controller {
            RouteValueDictionary routeValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);
            RouteValueDictionary htmlAttrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttrs["class"] == null) {
                htmlAttrs.Add("class", "ico[" + icoClass + "]");
            } else {
                htmlAttrs["class"] += "ico[" + icoClass + "]";
            }
            return helper.ActionLink(linkText, null, null, routeValues, htmlAttrs);
        }

        public static MvcHtmlString ActionLinkWselected(this HtmlHelper helper, string linkText, string actionName, string controlName) {
            if (helper.ViewContext.RouteData.Values["action"].ToString() == actionName &&
                helper.ViewContext.RouteData.Values["controller"].ToString() == controlName)
                return helper.ActionLink(linkText, actionName, controlName, new { Class = "selected" });
            return helper.ActionLink(linkText, actionName, controlName);
        }

        public static string IsSelected(this HtmlHelper helper, string controlName, string actionName) {
            var routeDatas = helper.ViewContext.RouteData.Values;
            if(routeDatas["controller"].ToString() == controlName){
                if(string.IsNullOrEmpty(actionName)) 
                    return "selected";
                else if(routeDatas["action"].ToString() == actionName)
                    return "selected";
            }
            return "";
        }
    }
}