using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace CrmDemoApp.Infrastructure
{
    public class CustomPrincipal : ICustomPrincipal
    {

        public CustomPrincipal(string username, string webServicetoken)
        {
            Identity = new GenericIdentity(username);
            WebServiceToken = webServicetoken;
        }


        public bool IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated &&
           !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
        }

        public IIdentity Identity { get; private set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string WebServiceToken { get; private set; }
    }

    public interface ICustomPrincipal : IPrincipal
    {
        string FirstName { get; set; }

        string LastName { get; set; }
    }
}