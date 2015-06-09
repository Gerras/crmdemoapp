using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Xrm;

namespace XrmUserStore
{
    public class UserLoginsTable
    {
        private readonly XrmServiceConnection _xrmServiceConnection;

        public UserLoginsTable(XrmServiceConnection connection)
        {
            _xrmServiceConnection = connection;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public void Delete(XrmIdentityUser user, UserLoginInfo login)
        {
            var userGuid = new Guid();
            var getUserLoginResult = _xrmServiceConnection.XrmServiceContext.new_contactLoginSet.Where(x => x.Id == userGuid).ToList().First();
            _xrmServiceConnection.XrmServiceContext.Delete("new_contactLogin", getUserLoginResult.Id);
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void DeleteAllReferences(string userId)
        {
            var userGuid = new Guid(userId);
            var getUserLoginResults = _xrmServiceConnection.XrmServiceContext.new_contactLoginSet.Where(x => x.new_ContactLoginProviderId.Id == userGuid).ToList();
            foreach (var userLogins in getUserLoginResults)
            {
                _xrmServiceConnection.XrmServiceContext.Delete("new_contactLogin", userLogins.Id);
            }

        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public void Insert(XrmIdentityUser user, UserLoginInfo login)
        {
            var userGuid = new Guid(user.Id);
            var getUserResult =
                _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id == userGuid)
                    .ToList()
                    .First();

            var contactLogin = new new_contactLogin
            {
                new_loginProvider = login.LoginProvider,
                new_ProviderKey = login.ProviderKey,
                new_ContactLoginProvider = getUserResult
            };

            _xrmServiceConnection.XrmServiceContext.AddObject(contactLogin);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();


        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            var userLoginResult =
                _xrmServiceConnection.XrmServiceContext.new_contactLoginSet.Where(
                    x => x.new_loginProvider == userLogin.LoginProvider && x.new_ProviderKey == userLogin.ProviderKey).ToList().FirstOrDefault();

            return userLoginResult == null ? null : userLoginResult.new_ContactLoginProviderId.Id.ToString();
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(string userId)
        {
            var userGuid = new Guid(userId);
            var userLogins =
                _xrmServiceConnection.XrmServiceContext.new_contactLoginSet.Where(
                    x => x.new_ContactLoginProviderId.Id == userGuid).ToList();

            return userLogins.Select(newContactLogin => new UserLoginInfo(newContactLogin.new_loginProvider, newContactLogin.new_ProviderKey)).ToList();
        }
    }
}
