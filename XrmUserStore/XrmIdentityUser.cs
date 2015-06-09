using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNet.Identity;
using Xrm;

namespace XrmUserStore
{
    public class XrmIdentityUser : IUser
    {
        public XrmIdentityUser(Contact contact)
        {
            Id = contact.Id.ToString();
            UserName = contact.new_UserName;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Email = contact.EMailAddress1;
        }

        public XrmIdentityUser()
        { }

        public Contact CreateContactFromIdentityUser()
        {
            return new Contact
            {
                FirstName = FirstName,
                LastName = LastName,
                new_UserName = UserName,
                EMailAddress1 = UserName,
                new_Password = PasswordHash,
                new_EmailConfirmation = EmailConfirmed,
                new_SecurityStamp = SecurityStamp,
                MobilePhone = PhoneNumber,
                new_PhoneNumberConfirmation = PhoneNumberConfirmed,
                new_TwoFactorEnabled = TwoFactorEnabled,
                new_LockOutEndDateUTC = LockoutEndDateUtc,
                new_LockoutEnabled = LockoutEnabled,
                new_AccessFailedCount = AccessFailedCount.ToString(),

            };
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<XrmIdentityUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        ///     The salted/hashed form of the user password
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        ///     A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     Is two factor enabled for the user
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public int AccessFailedCount { get; set; }
    }
}
