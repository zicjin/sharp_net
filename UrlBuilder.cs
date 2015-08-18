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

        public static void Build(string domain, string version, eControlDeploy config) {
            Version = version;
            Domain = domain;
            AssetsDeploy = String.Format("/assets/dist/{0}/{1}", Domain, Version);
            ControlDeploy = config;
        }

        private static eControlDeploy ControlDeploy = eControlDeploy.ByConfig;
        private static string AssetsDebug = "/assets/_site/dist";
        private static string Version = "1.0.0";
        private static string Domain = "zicjin-demo";
        private static string AssetsDeploy;

        public static bool IsDeploy() {
            if (ControlDeploy == eControlDeploy.ByConfig)
                return !HttpContext.Current.IsDebuggingEnabled;
            else
                return ControlDeploy == eControlDeploy.IsDeploy;
        }

        public static string AssetsUrl() {
            return IsDeploy() ? AssetsDeploy : AssetsDebug;
        }

        public static string AssetsUrl(string name) {
            return IsDeploy() ? String.Format("{0}/{1}.js", AssetsDeploy, name) : String.Format("{0}/{1}.js", AssetsDebug, name);
        }
    }
}