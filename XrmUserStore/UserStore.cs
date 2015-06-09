using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace XrmUserStore
{
    public class UserStore<TUser> : IUserLoginStore<TUser>, 
        IUserClaimStore<TUser>,
        IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IQueryableUserStore<TUser>,
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserTwoFactorStore<TUser, string>,
        IUserLockoutStore<TUser, string>,
        IUserStore<TUser>
        where TUser : XrmIdentityUser
    {
        private readonly UserTable<TUser> _userTable;
        private readonly RoleTable _roleTable;
        private readonly UserRolesTable _userRolesTable;
        private readonly UserClaimsTable _userClaimsTable;
        private readonly UserLoginsTable _userLoginsTable;

        private XrmServiceConnection XrmServiceConnection { get; set; }

        public UserStore(XrmServiceConnection xrmServiceConnection)
        {
            XrmServiceConnection = xrmServiceConnection;
            _userTable = new UserTable<TUser>(xrmServiceConnection);
            _roleTable = new RoleTable(xrmServiceConnection);
            _userRolesTable = new UserRolesTable(xrmServiceConnection);
            _userClaimsTable = new UserClaimsTable(xrmServiceConnection);
            _userLoginsTable = new UserLoginsTable(xrmServiceConnection);
        }

        public void Dispose()
        {
            if (XrmServiceConnection != null)
            {
                XrmServiceConnection.Dispose();
                XrmServiceConnection = null;
            }
        }

        public Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _userTable.Insert(user);
            user.Id = _userTable.GetUserId(user.UserName);
            
            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _userTable.Update(user);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TUser user)
        {
            if (user != null)
            {
                _userTable.Delete(user);
            }

            return Task.FromResult<object>(null);
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            var result = _userTable.GetUserById(userId);
            return result != null ? Task.FromResult(result) : Task.FromResult<TUser>(null);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            var result = _userTable.GetUserByName(userName);
            return result != null ? Task.FromResult(result) : Task.FromResult<TUser>(null);
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            _userLoginsTable.Insert(user, login);

            return Task.FromResult<object>(null);
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            _userLoginsTable.Delete(user, login);

            return Task.FromResult<Object>(null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var logins = _userLoginsTable.FindByUserId(user.Id);
            return Task.FromResult<IList<UserLoginInfo>>(logins);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userId = _userLoginsTable.FindUserIdByLogin(login);
            if (userId == null) return Task.FromResult<TUser>(null);
            var user = _userTable.GetUserById(userId);

            return user != null ? Task.FromResult(user) : Task.FromResult<TUser>(null);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<Object>(null);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            var passwordHash = _userTable.GetPasswordHash(user.Id);

            return Task.FromResult(passwordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            var hasPassword = !string.IsNullOrEmpty(_userTable.GetPasswordHash(user.Id));

            return Task.FromResult(Boolean.Parse(hasPassword.ToString()));
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            var identity = _userClaimsTable.FindByUserId(user.Id);

            return Task.FromResult<IList<Claim>>(identity.Claims.ToList());
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            _userClaimsTable.Insert(claim, user.Id);

            return Task.FromResult<object>(null);
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            _userClaimsTable.Delete(user, claim);

            return Task.FromResult<object>(null);
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            var roleId = _roleTable.GetRoleId(roleName);
            if (!string.IsNullOrEmpty(roleId))
            {
                _userRolesTable.Insert(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<string> roles = _userRolesTable.FindByUserId(user.Id);
            {
                if (roles != null)
                {
                    return Task.FromResult<IList<string>>(roles);
                }
            }

            return Task.FromResult<IList<string>>(null);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            var roles = _userRolesTable.FindByUserId(user.Id);
            {
                if (roles != null && roles.Contains(roleName))
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public IQueryable<TUser> Users
        {
            get { throw new NotImplementedException(); }
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            user.Email = email;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            var result = _userTable.GetUserByEmail(email);
            return Task.FromResult(result);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount++;
            _userTable.Update(user);

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            _userTable.Update(user);

            return Task.FromResult(0);
        }
    }
}
