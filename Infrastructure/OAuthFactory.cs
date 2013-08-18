using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using NetDimension.Weibo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace sharp_net.Infrastructure {
    public static class OAuthFactory {
        public static OAuth GetWeiboOAuth() {
            return new OAuth(ConfigurationManager.AppSettings["Weibo.AppKey"], ConfigurationManager.AppSettings["Weibo.AppSercet"], ConfigurationManager.AppSettings["Weibo.CallbackUrl"]);
        }

        public static IAuthenticationRequest GetGoogleOpenId() {
            OpenIdRelyingParty openid = new OpenIdRelyingParty();
            Uri callbackUrl = new Uri(ConfigurationManager.AppSettings["Google.CallbackUrl"]);
            IAuthenticationRequest request = openid.CreateRequest("https://www.google.com/accounts/o8/id", Realm.AutoDetect, callbackUrl);
            //Tell Google, what we want to have from the user:
            var fetch = new FetchRequest();
            //https://developers.google.com/accounts/docs/OpenID no avator ...
            fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
            fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
            fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);
            request.AddExtension(fetch);
            openid.Dispose();
            return request;
        }
    }
}