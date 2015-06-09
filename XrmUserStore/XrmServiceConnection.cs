using System;
using System.Configuration;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Client;
using Xrm;

namespace XrmUserStore
{
    public class XrmServiceConnection : IDisposable
    {
        private XrmServiceContext _xrmServiceContext;

        public XrmServiceContext XrmServiceContext { get; private set; }

        public XrmServiceConnection()
            : this("Xrm", "Username", "Password")
        {

        }

        public static XrmServiceConnection Create()
        {
            return new XrmServiceConnection();
        }

        public XrmServiceConnection(string connectionStringName, string username, string password)
        {
            var credentials = new ClientCredentials();
            credentials.UserName.UserName = ConfigurationManager.AppSettings[username];
            credentials.UserName.Password = ConfigurationManager.AppSettings[password];
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            var crmConnection = new CrmConnection(connectionStringName) { ClientCredentials = credentials };
            _xrmServiceContext = new XrmServiceContext(crmConnection);
            XrmServiceContext = _xrmServiceContext;
        }
        public void Dispose()
        {
            if (_xrmServiceContext != null)
            {
                _xrmServiceContext.Dispose();
                _xrmServiceContext = null;
            }
        }
    }
}
