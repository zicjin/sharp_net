using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace sharp_net {

    public enum eControlDeploy {
        IsDeploy = 0,
        NoDeploy = 1,
        ByConfig = 2
    }

    public static class UrlBuilder {

        static UrlBuilder() {
            ControlDeploy = eControlDeploy.ByConfig;
            Stamp = "v=121127";
            StaticDeploy = "/assets_dist";
        }

        public static eControlDeploy ControlDeploy { get; set; }
        public static string Stamp { get; set; }
        public static string StaticDeploy { get; set; }

        public static bool IsDeploy() {
            if (ControlDeploy == eControlDeploy.ByConfig)
                return !HttpContext.Current.IsDebuggingEnabled;
            else
                return ControlDeploy == eControlDeploy.IsDeploy ? true : false;
        }

        public static string StaticUrlNostamp(string file) {
            StringBuilder Url = new StringBuilder();
            return Url.AppendFormat("{0}{1}", IsDeploy() ? StaticDeploy : "/assets", file).ToString();
        }

        public static string StaticUrl(string file) {
            StringBuilder Url = new StringBuilder();
            return Url.AppendFormat("{0}{1}?{2}", IsDeploy() ? StaticDeploy : "/assets", file, Stamp).ToString();
        }

        public static string ScriptDoc(string file) {
            StringBuilder Path = new StringBuilder();
            return Path.AppendFormat("<script type='text/javascript' src='{0}'></script>", StaticUrl(file)).ToString();
        }

        public static string CssDoc(string file) {
            StringBuilder Path = new StringBuilder();
            return Path.AppendFormat("<link rel='stylesheet' type='text/css' href='{0}' />", StaticUrl(file)).ToString();
        }

        #region Razor
        public static IHtmlString StaticUrl(this HtmlHelper helper, string file) {
            return helper.Raw(StaticUrl(file));
        }
        public static IHtmlString ScriptDoc(this HtmlHelper helper, string file) {
            return helper.Raw(ScriptDoc(file));
        }
        public static IHtmlString CssDoc(this HtmlHelper helper, string file) {
            return helper.Raw(CssDoc(file));
        }
        #endregion
    }
}