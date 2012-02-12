﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using System.Xml.Serialization;
using System.IO;

namespace CSharpcommon {
    public class UserFilter : ActionFilterAttribute {
        public override void OnActionExecuted(ActionExecutedContext context) {
            if (context.HttpContext.User.Identity.IsAuthenticated) {
                UserIdentity userModel = context.HttpContext.User.GetIdentityUser();
                context.Controller.ViewBag.UserModel = userModel;
            }
            base.OnActionExecuted(context);
        }
    }

    public interface IFormsAuthentication {
        void Signout();
        void SetAuthCookie(HttpContextBase httpContext, FormsAuthenticationTicket authenticationTicket);
        void SetAuthCookie(HttpContext httpContext, FormsAuthenticationTicket authenticationTicket);
        FormsAuthenticationTicket Decrypt(string encryptedTicket);
    }

    public static class SecurityExtensions {
        public static void PostAuthenticateRequestHandler(object sender, EventArgs e) {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !String.IsNullOrEmpty(authCookie.Value)) {
                var formsAuthentication = DependencyResolver.Current.GetService<IFormsAuthentication>();
                var ticket = formsAuthentication.Decrypt(authCookie.Value);
                var userId = new UserIdentity(ticket);
                string[] userRoles = { userId.RoleName };
                HttpContext.Current.User = new GenericPrincipal(userId, userRoles);
                formsAuthentication.SetAuthCookie(HttpContext.Current, ticket);
            }
        }

        public static UserIdentity GetIdentityUser(this IPrincipal principal) {
            if (principal.Identity is UserIdentity)
                return (UserIdentity)principal.Identity;
            else
                return new UserIdentity(string.Empty, new UserSerialize());
        }
    }

    [Serializable]
    public class UserIdentity : IIdentity {
        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string RoleName { get; private set; }

        public UserIdentity(string name, UserSerialize userSer) {
            if (userSer == null) throw new ArgumentNullException("userInfo");
            this.Name = name;
            this.Email = userSer.Email;
            this.UserId = userSer.UserId;
            this.RoleName = userSer.RoleName;
        }
        public UserIdentity(FormsAuthenticationTicket ticket)
            : this(ticket.Name, UserSerialize.FromString(ticket.UserData)) {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
        }
        public bool IsAuthenticated {
            get { return true; }
        }
        public string AuthenticationType {
            get { return "stackFluentForms"; }
        }
    }

    public class UserSerialize {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string RoleName { get; set; }
        public override string ToString() {
            XmlSerializer serializer = new XmlSerializer(typeof(UserSerialize));
            using (var stream = new StringWriter()) {
                serializer.Serialize(stream, this);
                return stream.ToString();
            }
        }
        public static UserSerialize FromString(string userContextData) {
            XmlSerializer serializer = new XmlSerializer(typeof(UserSerialize));
            using (var stream = new StringReader(userContextData)) {
                return serializer.Deserialize(stream) as UserSerialize;
            }
        }
    }
}