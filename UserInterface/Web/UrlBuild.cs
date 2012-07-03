using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace zic_dotnet {

    public static class UrlBuild {

        static UrlBuild() {
            ControlDeploy = eControlDeploy.ByDebug;
            JsHead = "/js/";
            JsHeadDeploy = "/js/__build/";
            CssHead = "/css/";
            CssHeadDeploy = "/css/__build/";
            ImgHead = "/css/images/";
            ImgHeadDeploy = "/css/images/";
            //JsRepoFix = "zic-js";
            //JsRepoDeployFix = "__build";
            //CssRepoFix = "zic-css";
            //CssRepoDeployFix = "__build";
        }

        public enum eControlDeploy {
            IsDeploy = 0,
            NoDeploy = 1,
            ByDebug = 2
        }

        public static eControlDeploy ControlDeploy { get; set; }

        public static string JsHead { get; set; }

        public static string JsHeadDeploy { get; set; }

        public static string CssHead { get; set; }

        public static string CssHeadDeploy { get; set; }

        public static string ImgHead { get; set; }

        public static string ImgHeadDeploy { get; set; }

        public static string JsRepoFix { get; set; }

        public static string JsRepoDeployFix { get; set; }

        public static string CssRepoFix { get; set; }

        public static string CssRepoDeployFix { get; set; }

        public static bool IsDeploy() {
            if (ControlDeploy == eControlDeploy.ByDebug)
                return !HttpContext.Current.IsDebuggingEnabled;
            else
                return ControlDeploy == eControlDeploy.IsDeploy ? true : false;
        }

        public static string ScriptUrl(string file) {
            //if (!String.IsNullOrEmpty(JsRepoFix) && file.Contains(JsRepoFix) && IsDeploy())
            //    return (JsHeadDeploy + file).Replace(JsRepoDeployFix + "/" + JsRepoFix, JsRepoFix + "/" + JsRepoDeployFix);
            return (IsDeploy() ? JsHeadDeploy : JsHead) + file;
        }

        public static string ScriptDoc(string file) {
            StringBuilder Path = new StringBuilder();
            return Path.AppendFormat("<script type='text/javascript' src='{0}'></script>", ScriptUrl(file)).ToString();
        }

        public static IHtmlString ScriptUrl(this HtmlHelper helper, string file) {
            return helper.Raw(ScriptUrl(file));
        }

        public static IHtmlString ScriptDoc(this HtmlHelper helper, string file) {
            return helper.Raw(ScriptDoc(file));
        }

        public static string CssUrl(string file) {
            //if (!String.IsNullOrEmpty(CssRepoFix) && file.Contains(CssRepoFix) && IsDeploy())
            //    return (CssHeadDeploy + file).Replace(CssRepoDeployFix + "/" + CssRepoFix, CssRepoFix + "/" + CssRepoDeployFix);
            return (IsDeploy() ? CssHeadDeploy : CssHead) + file;
        }

        public static string CssDoc(string file) {
            StringBuilder Path = new StringBuilder();
            return Path.AppendFormat("<link rel='stylesheet' type='text/css' href='{0}' />", CssUrl(file)).ToString();
        }

        public static IHtmlString CssUrl(this HtmlHelper helper, string file) {
            return helper.Raw(CssUrl(file));
        }

        public static IHtmlString CssDoc(this HtmlHelper helper, string file) {
            return helper.Raw(CssDoc(file));
        }

        public static string ImgUrl(string file) {
            return (IsDeploy() ? ImgHeadDeploy : ImgHead) + file;
        }

        public static IHtmlString ImgUrl(this HtmlHelper helper, string file) {
            return helper.Raw(ImgUrl(file));
        }
    }
}