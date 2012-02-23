using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CSharpcommon {
    public static class UrlBuild {
        static UrlBuild() {
            ByDebug = true;
            JsHead = "/js/";
            JsHeadOnline = "/js/__build/";
            CssHead = "/css/";
            CssHeadOnline = "/css/__build/";
            ImgHead = "/css/img/";
            ImgHeadOnline = "/css/img/";
        }
        public static bool ByDebug { get; set; }
        public static string JsHead { get; set; }
        public static string JsHeadOnline { get; set; }
        public static string CssHead { get; set; }
        public static string CssHeadOnline { get; set; }
        public static string ImgHead { get; set; }
        public static string ImgHeadOnline { get; set; }

        public static string ScriptUrl(string file) {
            return (HttpContext.Current.IsDebuggingEnabled && ByDebug ? JsHead : JsHeadOnline) + file;
        }
        public static string ScriptDoc(string file) {
            return "<script type='text/javascript' src='" + ScriptUrl(file) + "' />";
        }
        public static IHtmlString ScriptUrl(this HtmlHelper helper, string file) {
            return helper.Raw(ScriptUrl(file));
        }
        public static IHtmlString ScriptDoc(this HtmlHelper helper, string file) {
            return helper.Raw(ScriptDoc(file));
        }

        public static string CssUrl(string file) {
            return (HttpContext.Current.IsDebuggingEnabled && ByDebug ? CssHead : CssHeadOnline) + file;
        }
        public static string CssDoc(string file) {
            return "<link rel='stylesheet' type='text/css' href='" + CssUrl(file) +"' />";
        }
        public static IHtmlString CssUrl(this HtmlHelper helper, string file) {
            return helper.Raw(CssUrl(file));
        }
        public static IHtmlString CssDoc(this HtmlHelper helper, string file) {
            return helper.Raw(CssDoc(file));
        }

        public static string ImgUrl(string file) {
            return (HttpContext.Current.IsDebuggingEnabled && ByDebug ? ImgHead : ImgHeadOnline) + file;
        }
        public static IHtmlString ImgDoc(this HtmlHelper helper, string file) {
            return helper.Raw(ImgUrl(file));
        }
    }
}