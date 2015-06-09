using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CrmDemoApp.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthenticated = base.AuthorizeCore(httpContext);
            if (!isAuthenticated) return false;
            var cookieName = FormsAuthentication.FormsCookieName;
            if (!httpContext.User.Identity.IsAuthenticated ||
                httpContext.Request.Cookies == null || 
                httpContext.Request.Cookies[cookieName] == null)
            {
                return false;
            }

            var authCookie = httpContext.Request.Cookies[cookieName];
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            // This is where you can read the userData part of the authentication
            // cookie and fetch the token
            if (authTicket == null) return true;
            var webServiceToken = authTicket.UserData;

            var userPrincipal = new CustomPrincipal("test", webServiceToken);

            httpContext.User = userPrincipal;
            return true;
        }
    }
}