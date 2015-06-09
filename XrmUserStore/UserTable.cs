using System;
using System.Linq;

namespace XrmUserStore
{
    public class UserTable<TUser> where TUser : XrmIdentityUser
    {
        private readonly XrmServiceConnection _xrmServiceConnection;

        public UserTable(XrmServiceConnection connection)
        {
            _xrmServiceConnection = connection;
        }

        public string GetUserNameByUserId(string userId)
        {
            var userGuid = new Guid(userId);
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id == userGuid)
                    .ToList()
                    .FirstOrDefault();
            return getUserResult == null ? null : getUserResult.new_UserName;
        }

        public string GetUserId(string userName)
        {
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.new_UserName == userName)
                    .ToList()
                    .FirstOrDefault();
            return getUserResult == null ? null : getUserResult.Id.ToString();
        }

        public TUser GetUserById(string userId)
        {
            var guid = new Guid(userId);
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id == guid).ToList().FirstOrDefault();
            if (getUserResult == null) return null;
            var user = (TUser)Activator.CreateInstance(typeof(TUser));
            user.Id = getUserResult.Id.ToString();
            user.UserName = getUserResult.new_UserName;
            user.PasswordHash = getUserResult.new_Password;
            user.Email = getUserResult.EMailAddress1;
            user.SecurityStamp = getUserResult.new_SecurityStamp;
            if (getUserResult.new_EmailConfirmation != null)
                user.EmailConfirmed = (bool)getUserResult.new_EmailConfirmation;
            user.PhoneNumber = getUserResult.MobilePhone;
            if (getUserResult.new_PhoneNumberConfirmation != null)
                user.PhoneNumberConfirmed = (bool)getUserResult.new_PhoneNumberConfirmation;
            if (getUserResult.new_LockoutEnabled != null) user.LockoutEnabled = (bool)getUserResult.new_LockoutEnabled;
            user.LockoutEndDateUtc = getUserResult.new_LockOutEndDateUTC;
            user.AccessFailedCount = Convert.ToInt32(getUserResult.new_AccessFailedCount);
            return user;

        }

        public TUser GetUserByName(string userName)
        {
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.new_UserName == userName).ToList().FirstOrDefault();
            if (getUserResult == null) return null;
            var user = (TUser)Activator.CreateInstance(typeof(TUser));
            user.Id = getUserResult.Id.ToString();
            user.UserName = getUserResult.new_UserName;
            user.PasswordHash = getUserResult.new_Password;
            user.Email = getUserResult.EMailAddress1;
            user.SecurityStamp = getUserResult.new_SecurityStamp;
            if (getUserResult.new_EmailConfirmation != null)
                user.EmailConfirmed = (bool)getUserResult.new_EmailConfirmation;
            user.PhoneNumber = getUserResult.MobilePhone;
            if (getUserResult.new_PhoneNumberConfirmation != null)
                user.PhoneNumberConfirmed = (bool)getUserResult.new_PhoneNumberConfirmation;
            if (getUserResult.new_LockoutEnabled != null) user.LockoutEnabled = (bool)getUserResult.new_LockoutEnabled;
            user.LockoutEndDateUtc = getUserResult.new_LockOutEndDateUTC;
            user.AccessFailedCount = Convert.ToInt32(getUserResult.new_AccessFailedCount);
            return user;
        }

        public TUser GetUserByEmail(string email)
        {
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.EMailAddress1 == email).ToList().FirstOrDefault();
            if (getUserResult == null) return null;
            var user = (TUser)Activator.CreateInstance(typeof(TUser));
            user.Id = getUserResult.Id.ToString();
            user.UserName = getUserResult.new_UserName;
            user.PasswordHash = getUserResult.new_Password;

            user.Email = getUserResult.EMailAddress1;
            user.SecurityStamp = getUserResult.new_SecurityStamp;
            if (getUserResult.new_EmailConfirmation != null)
                user.EmailConfirmed = (bool) getUserResult.new_EmailConfirmation;
            user.PhoneNumber = getUserResult.MobilePhone;
            if (getUserResult.new_PhoneNumberConfirmation != null)
                user.PhoneNumberConfirmed = (bool) getUserResult.new_PhoneNumberConfirmation;
            if (getUserResult.new_LockoutEnabled != null) user.LockoutEnabled = (bool) getUserResult.new_LockoutEnabled;
            user.LockoutEndDateUtc = getUserResult.new_LockOutEndDateUTC;
            user.AccessFailedCount = Convert.ToInt32(getUserResult.new_AccessFailedCount);
            return user;
        }

        public string GetPasswordHash(string userId)
        {
            var userGuid = new Guid(userId);
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id == userGuid).ToList().FirstOrDefault();
            return getUserResult == null ? null : getUserResult.new_Password;
        }

        public void SetPasswordHash(string userId, string passwordHash)
        {
            var userGuid = new Guid(userId);
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id == userGuid).ToList().First();
            getUserResult.new_Password = passwordHash;
            _xrmServiceConnection.XrmServiceContext.Update(getUserResult);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }

        public string GetSecurityStamp(string userId)
        {
            var userGuid = new Guid(userId);
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id == userGuid).ToList().First();
            return getUserResult.new_SecurityStamp;
        }

        public void Insert(TUser user)
        {
            var identityUser = user.CreateContactFromIdentityUser();
            _xrmServiceConnection.XrmServiceContext.AddObject(identityUser);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }

        private void Delete(string userId)
        {
            var userGuid = new Guid(userId);
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id == userGuid).ToList().First();
            _xrmServiceConnection.XrmServiceContext.Delete("Contact", getUserResult.Id);
        }

        public void Delete(TUser user)
        {
            Delete(user.Id);
        }

        public void Update(TUser user)
        {
            var userGuid = new Guid(user.Id);
            var getUserResult = _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id == userGuid).ToList().First();
            getUserResult.new_UserName = user.UserName;
            getUserResult.new_Password = user.PasswordHash;
            getUserResult.EMailAddress1 = user.Email;
            getUserResult.FirstName = user.FirstName;
            getUserResult.LastName = user.LastName;
            _xrmServiceConnection.XrmServiceContext.Update(getUserResult);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }
    }
}
