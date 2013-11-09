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

        public static IList<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items,
            Func<T, string> getText, Func<T, string> getValue, string selectValue) {
            return items.OrderBy(i => getText(i))
            .Select(i => new SelectListItem {
                Selected = (getValue(i) == selectValue),
                Text = getText(i),
                Value = getValue(i)
            }).ToList();
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj) {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObj);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>(this TEnum enumObj) {
            return from TEnum e in Enum.GetValues(typeof(TEnum))
                   select new SelectListItem() {
                       Selected = (Convert.ToInt32(e) == Convert.ToInt32(enumObj)), //使用DropDownListFor的话是不需要指定Selected的
                       Text = e.ToString(),
                       Value = Convert.ToInt32(e).ToString()
                   };
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>(this TEnum enumObj, object attachEnum) {
            return from TEnum e in Enum.GetValues(typeof(TEnum))
                   select new SelectListItem() {
                       Selected = (Convert.ToInt32(e) == Convert.ToInt32(enumObj)),
                       Text = e.GetAttachedDataFromObj<string>(attachEnum),
                       Value = Convert.ToInt32(e).ToString()
                   };
        }

        public static IEnumerable<GroupedSelectListItem> ToSelectGroupListItems<TEnum>(this Type enumObj, TEnum enumGroup, object attachEnum, IEnumerable<string> selvals) {
            return from Object e in Enum.GetValues(enumObj)
                   select new GroupedSelectListItem() {
                       Text = e.GetAttachedDataFromObj<string>(attachEnum),
                       Value = e.ToString(),
                       GroupKey = enumGroup.ToString(),
                       GroupName = enumGroup.GetAttachedDataFromObj<string>(attachEnum),
                       Selected = selvals == null ? false : selvals.Contains(e.ToString())
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

        public static MvcHtmlString ActionLinkWico(this HtmlHelper helper, string linkText, string actionName, string controlName, object routeValues, object htmlAttributes, string icoClass) {
            RouteValueDictionary htmlAttrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttrs["class"] == null) {
                htmlAttrs.Add("class", "ico-" + icoClass);
            } else {
                htmlAttrs["class"] += " ico-" + icoClass;
            }
            return helper.ActionLink(linkText, actionName, controlName, routeValues, htmlAttrs);
        }

        public static MvcHtmlString ActionLinkWselected(this HtmlHelper helper, string linkText, string actionName, string controlName) {
            if (helper.ViewContext.RouteData.Values["action"].ToString() == actionName &&
                helper.ViewContext.RouteData.Values["controller"].ToString() == controlName)
                return helper.ActionLink(linkText, actionName, controlName, new { Class = "selected" });
            return helper.ActionLink(linkText, actionName, controlName);
        }

        public static string IsSelected(this HtmlHelper helper, string controlName, string actionName) {
            var routeDatas = helper.ViewContext.RouteData.Values;
            if (routeDatas["controller"].ToString() == controlName) {
                if (string.IsNullOrEmpty(actionName))
                    return "selected";
                else if (routeDatas["action"].ToString() == actionName)
                    return "selected";
            }
            return "";
        }
    }
}