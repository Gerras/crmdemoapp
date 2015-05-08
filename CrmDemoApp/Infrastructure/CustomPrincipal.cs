using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace CrmDemoApp.Infrastructure
{
    public class CustomPrincipal : IPrincipal
    {
        public bool IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated &&
           !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
        }

        public IIdentity Identity { get; private set; }

        public string WebserviceToken { get; set; }
    }
}