using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Xrm;
using Task = System.Threading.Tasks.Task;

namespace XrmUserStore
{
    public class RoleStore<TRole> : IQueryableRoleStore<TRole> where TRole : XrmIdentityRole
    {

        private RoleTable roleTable;
        public XrmServiceConnection XrmServiceConnection { get; private set; }

        public RoleStore()
        {
           new RoleStore<TRole>(new XrmServiceConnection());
        }

        //public RoleStore(MySQLDatabase database)
        //{
        //    Database = database;
        //    roleTable = new RoleTable(database);
        //}

        public RoleStore(XrmServiceConnection xrmServiceConnection)
        {
            XrmServiceConnection = xrmServiceConnection;
            roleTable = new RoleTable(xrmServiceConnection);
        }

        public void Dispose()
        {
            if (XrmServiceConnection != null)
            {
                XrmServiceConnection.Dispose();
                XrmServiceConnection = null;
            }
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            roleTable.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            roleTable.Update(role);

            return Task.FromResult<Object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            roleTable.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            var result = roleTable.GetRoleById(roleId) as TRole;

            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            var result = roleTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult(result);
        }

        public IQueryable<TRole> Roles
        {
            get { throw new NotImplementedException(); }
        }
    }
}
