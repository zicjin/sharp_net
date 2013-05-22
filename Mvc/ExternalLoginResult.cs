using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace sharp_net.Mvc {
    public class ExternalLoginResult : ActionResult {
        public ExternalLoginResult(string provider, string returnUrl) {
            Provider = provider;
            ReturnUrl = returnUrl;
        }

        public string Provider { get; private set; }
        public string ReturnUrl { get; private set; }

        public override void ExecuteResult(ControllerContext context) {
            OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
        }
    }
}
