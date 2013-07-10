using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sharp_net.Mvc {
    public class HandleErrorForJsonAttribute : HandleErrorAttribute {
        public override void OnException(ExceptionContext filterContext) {
            if (filterContext == null) {
                throw new ArgumentNullException("filterContext");
            }
            
            if (!filterContext.IsChildAction) { //&& filterContext.HttpContext.IsCustomErrorEnabled 应该不需要
                Exception innerException = filterContext.Exception;
                if ((new HttpException(null, innerException).GetHttpCode() == 500) && this.ExceptionType.IsInstanceOfType(innerException)) {
                    filterContext.Result = new JsonResult {
                        Data = new { result = false, error = innerException.Message },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.StatusCode = 500;
                }
            }

        }
    }
}