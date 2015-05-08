using System;
using System.Configuration;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Client;
using Xrm;

namespace CrmDemoApp.Infrastructure
{
    public class XrmServiceConnector : IDisposable
    {
        private readonly string _userName;
        private readonly string _password;
        private XrmServiceContext _xrmServiceContext;

        public XrmServiceContext XrmServiceContext
        {
            get { return _xrmServiceContext ?? (_xrmServiceContext = new XrmServiceContext(CreateCrmConnection())); }
        }

        public XrmServiceConnector()
        {
            _userName = ConfigurationManager.AppSettings["userName"];
            _password = ConfigurationManager.AppSettings["password"];
            CreateCrmConnection();
        }

        private CrmConnection CreateCrmConnection()
        {
            var credentials = new ClientCredentials();
            credentials.UserName.UserName = _userName;
            credentials.UserName.Password = _password;
            credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            return new CrmConnection("Xrm") {ClientCredentials = credentials};
        }

        public XrmServiceContext CreateXrmServiceConnector()
        {
            return _xrmServiceContext ?? (_xrmServiceContext = new XrmServiceContext(CreateCrmConnection()));
        }

        public void Dispose()
        {
            if (_xrmServiceContext != null)
            {
                _xrmServiceContext.Dispose();
            }
        }
    }
}


        //private ShipperServicesDb _context;

        //protected ShipperServicesDb Context
        //{
        //    get { return _context ?? (_context = new ShipperServicesDb()); }
        //}

        //protected BaseService()
        //{
        //}

        //public void Dispose()
        //{
        //    if (_context != null)
        //    {
        //        _context.Dispose();    
        //    }
            
            
        //}