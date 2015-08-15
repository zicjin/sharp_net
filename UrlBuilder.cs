using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace sharp_net.Mvc {

    public enum eControlDeploy {
        IsDeploy,
        NoDeploy,
        ByConfig
    }

    public static class UrlBuild {

        static UrlBuild() {
            ControlDeploy = eControlDeploy.ByConfig;
            AssetsDeploy = "/assets_dist";
            Version = "1.0";
        }

        public static eControlDeploy ControlDeploy { get; set; }
        public static string AssetsDeploy { get; set; }
        public static string Version { get; set; }

        public static bool IsDeploy() {
            if (ControlDeploy == eControlDeploy.ByConfig)
                return !HttpContext.Current.IsDebuggingEnabled;
            else
                return ControlDeploy == eControlDeploy.IsDeploy;
        }

        public static string AssetsUrl() {
            return IsDeploy() ? AssetsDeploy : "/assets";
        }

        public static string AssetsUrlOutExt(string name) {
            return IsDeploy() ? String.Format("{0}/{1}.js", AssetsDeploy, name) : "/assets/" + name;
        }
    }
}