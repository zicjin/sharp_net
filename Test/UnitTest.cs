using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace sharp_net.Test {

    //http://blog.zhaojie.me/2009/04/cannot-have-too-many-helper-methods.html
    //辅助方法不嫌多
    public static class AssertHelpers {
        public static T Is<T>(this object result) {
            Assert.IsTrue(
                result is T,
                "actionResult is expected to be '{0}' but '{1}'", typeof(T), result.GetType());
            return (T)result;
        }

        public static T IsView<T>(this T result, string viewName, string masterName) where T : ViewResult {
            viewName = viewName ?? "";
            masterName = masterName ?? "";

            Assert.IsTrue(
                String.Equals(viewName, result.ViewName, StringComparison.InvariantCultureIgnoreCase),
                "The view name is expected to be {0} but {1}",
                viewName == "" ? "the default one" : "'" + viewName + "'",
                result.ViewName == "" ? "the default one" : "'" + result.ViewName + "'");

            Assert.IsTrue(
                String.Equals(masterName, result.MasterName, StringComparison.InvariantCultureIgnoreCase),
                "The master name is expected to be {0} but {1}",
                masterName == "" ? "the default one" : "'" + masterName + "'",
                result.MasterName == "" ? "the default one" : "'" + result.MasterName + "'");

            return result;
        }
    }
}
