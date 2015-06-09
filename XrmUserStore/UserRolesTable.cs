using System;
using System.Collections.Generic;
using System.Linq;
using Xrm;

namespace XrmUserStore
{
    public class UserRolesTable
    {
        private readonly XrmServiceConnection _xrmServiceConnection;

        public UserRolesTable(XrmServiceConnection xrmServiceConnection)
        {
            _xrmServiceConnection = xrmServiceConnection;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(string userId)
        {
            var guid = new Guid(userId);
            var getUserRoles =
                _xrmServiceConnection.XrmServiceContext.new_contactuserroleSet.Where(
                    x => x.new_contactuserroleId == guid).ToList();

            return getUserRoles.Select(userRoles => userRoles.new_name).ToList();
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(string userId)
        {

            var guid = new Guid(userId);
            var getUserRoles =
                _xrmServiceConnection.XrmServiceContext.new_contactuserroleSet.Where(
                    x => x.new_contactuserroleId == guid).ToList();
            foreach (var contactUserRole in getUserRoles)
            {
                _xrmServiceConnection.XrmServiceContext.Delete("new_contactuserrole", contactUserRole.Id);
            }
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public void Insert(XrmIdentityUser user, string roleId)
        {
            var userGuid = new Guid(user.Id);
            var roleGuid = new Guid(roleId);
            var getUser = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(
                    x => x.Id == userGuid).ToList().First();
            var getUserRole =
                _xrmServiceConnection.XrmServiceContext.new_ContactRoleSet.Where(x => x.Id == roleGuid).ToList().First();

            var userRoles = new new_contactuserrole
            {
                new_new_contactrole_new_contactuserrole = getUserRole,
                new_contact_new_contactuserrole = getUser
            };
            _xrmServiceConnection.XrmServiceContext.AddObject(userRoles);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }

    }
}
