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
            AssetsDeploy = "/assets_dist";
        }

        public static eControlDeploy ControlDeploy { get; set; }
        public static string Stamp { get; set; }
        public static string AssetsDeploy { get; set; }

        public static bool IsDeploy() {
            if (ControlDeploy == eControlDeploy.ByConfig)
                return !HttpContext.Current.IsDebuggingEnabled;
            else
                return ControlDeploy == eControlDeploy.IsDeploy ? true : false;
        }

        public static string AssetsUrl() {
            return IsDeploy() ? AssetsDeploy : "/assets";
        }

        #region Doc
        public static string ScriptDoc(string file) {
            return new StringBuilder().AppendFormat("<script type='text/javascript' src='{0}{1}?{2}'></script>", IsDeploy() ? AssetsDeploy + "/src" : "/assets/src", file, Stamp).ToString();
        }

        public static string CssDoc(string file) {
            return new StringBuilder().AppendFormat("<link rel='stylesheet' type='text/css' href='{0}{1}?{2}' />", IsDeploy() ? AssetsDeploy + "/Content" : "/assets/Content", file, Stamp).ToString();
        }

        public static IHtmlString JsDoc(this HtmlHelper helper, string file) {
            return helper.Raw(ScriptDoc(file));
        }
        public static IHtmlString CssDoc(this HtmlHelper helper, string file) {
            return helper.Raw(CssDoc(file));
        }
        #endregion
    }
}