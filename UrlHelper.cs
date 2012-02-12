using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CSharpcommon {
    public static class UrlHelper {
        public static string Script(string path) {
            return HttpContext.Current.IsDebuggingEnabled ? "" : path + "/";
        }

        public static string Script(this HtmlHelper helper, string path) {
            return helper.ViewContext.HttpContext.IsDebuggingEnabled ? "" : path + "/";
        }
    }
}