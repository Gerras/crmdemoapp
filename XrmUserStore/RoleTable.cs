using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xrm;

namespace XrmUserStore
{
    public class RoleTable
    {
        private readonly XrmServiceConnection _xrmServiceConnection;

        public RoleTable(XrmServiceConnection xrmServiceConnection)
        {
            _xrmServiceConnection = xrmServiceConnection;
        }

        public void Delete(string roleId)
        {
            var getUserResult = _xrmServiceConnection.XrmServiceContext.new_ContactRoleSet.Where(x => x.Id.ToString() == roleId).ToList().First();
            _xrmServiceConnection.XrmServiceContext.Delete("ContactRoleSet", getUserResult.Id);
        }

        public void Insert(XrmIdentityRole role)
        {
            var xrmRole = new new_ContactRole
            {
                new_name = role.Name
            };
            _xrmServiceConnection.XrmServiceContext.AddObject(xrmRole);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }

        public string GetRoleName(string roleId)
        {
            var getUserResult = _xrmServiceConnection.XrmServiceContext.new_ContactRoleSet.Where(x => x.Id.ToString() == roleId).ToList().First();
            return getUserResult.new_name;
        }

        public string GetRoleId(string roleName)
        {
            var getUserResult = _xrmServiceConnection.XrmServiceContext.new_ContactRoleSet.Where(x => x.new_name == roleName).ToList().First();
            return getUserResult.Id.ToString();
        }

        public XrmIdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);
            XrmIdentityRole role = null;
            if (roleName != null)
            {
                role = new XrmIdentityRole(roleName, roleId);
            }

            return role;
        }

        public XrmIdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            XrmIdentityRole role = null;

            if (roleId != null)
            {
                role = new XrmIdentityRole(roleName, roleId);
            }

            return role;
        }

        public void Update(XrmIdentityRole role)
        {
            var getUserResult = _xrmServiceConnection.XrmServiceContext.new_ContactRoleSet.Where(x => x.Id.ToString() == role.Id).ToList().First();
            getUserResult.new_name = role.Name;
            _xrmServiceConnection.XrmServiceContext.Update(getUserResult);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }
    }
}
