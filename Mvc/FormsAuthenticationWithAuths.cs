using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Serialization;

namespace sharp_net.Mvc {

    //扩展，得到支持Roles与Auths的IFormsAuthentication
    [Serializable]
    public class UserIdentity : IIdentity {

        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Roles { get; set; }
        public string Auths { get; set; }

        public UserIdentity() {
        }
        public UserIdentity(FormsAuthenticationTicket ticket) {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
            this.Email = ticket.Name;
            UserIdentity user = UserIdentity.FromString(ticket.UserData);
            this.Id = user.Id;
            this.Name = user.Name;
            this.Roles = user.Roles;
            this.Auths = user.Auths;
        }

        public bool IsAuthenticated {
            get { return true; }
        }

        public string AuthenticationType {
            get { return "zicForms"; }
        }

        public override string ToString() {
            XmlSerializer serializer = new XmlSerializer(typeof(UserIdentity));
            using (var stream = new StringWriter()) {
                serializer.Serialize(stream, this);
                return stream.ToString();
            }
        }

        public static UserIdentity FromString(string userContextData) {
            XmlSerializer serializer = new XmlSerializer(typeof(UserIdentity));
            using (var stream = new StringReader(userContextData)) {
                return serializer.Deserialize(stream) as UserIdentity;
            }
        }
    }

    public interface IFormsAuthentication {
        void Signout();
        void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket);
        void SetAuthCookie(HttpContext httpContext, FormsAuthenticationTicket authenticationTicket);
        FormsAuthenticationTicket Decrypt(string encryptedTicket);
    }

    public class FormsAuthenticationWithAuths : IFormsAuthentication {

        public void SetAuthCookie(string key, bool persistent) {
            FormsAuthentication.SetAuthCookie(key, persistent);
        }

        public void Signout() {
            FormsAuthentication.SignOut();
        }

        public void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket) {
            var encryptedTicket = FormsA﻿﻿uthentication.Encrypt(authenticationTicket);
            httpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { Expires = CalculateCookieExpirationDate() });
        }

        public void SetAuthCookie(HttpContext httpContext, FormsAuthenticationTicket authenticationTicket) {
            var encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
            httpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { Expires = CalculateCookieExpirationDate() });
        }

        private static DateTime CalculateCookieExpirationDate() {
            return DateTime.Now.Add(FormsAuthentication.Timeout);
        }

        public FormsAuthenticationTicket Decrypt(string encryptedTicket) {
            return FormsAuthentication.Decrypt(encryptedTicket);
        }
    }

    public static class SecurityExtensions {
        //MvcApplication:
        //public override void Init() {
        //    this.PostAuthenticateRequest += SecurityExtensions.PostAuthenticateRequestHandler;
        //    base.Init();
        //}
        public static void PostAuthenticateRequestHandler(object sender, EventArgs e) {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !String.IsNullOrEmpty(authCookie.Value)) {
                var formsAuthentication = DependencyResolver.Current.GetService<IFormsAuthentication>();
                var ticket = formsAuthentication.Decrypt(authCookie.Value);
                var userIde = new UserIdentity(ticket);
                string[] userRoles = userIde.Roles.Split(',');
                HttpContext.Current.User = new GenericPrincipal(userIde, userRoles);
                formsAuthentication.SetAuthCookie(HttpContext.Current, ticket);
            }
        }

        public static UserIdentity GetIdentityUser(this IPrincipal principal) {
            if (principal.Identity is UserIdentity)
                return (UserIdentity)principal.Identity;
            else
                return new UserIdentity();
        }
    }

    public class UserInject : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            if (context.HttpContext.User.Identity.IsAuthenticated)
                context.Controller.ViewBag.UserModel = context.HttpContext.User.GetIdentityUser();
            base.OnActionExecuting(context);
        }
    }

}