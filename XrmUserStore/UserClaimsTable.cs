using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xrm;

namespace XrmUserStore
{
    public class UserClaimsTable
    {
        private readonly XrmServiceConnection _xrmServiceConnection;


        public UserClaimsTable(XrmServiceConnection serviceConnection)
        {
            _xrmServiceConnection = serviceConnection;
        }


        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(string userId)
        {
            var claims = new ClaimsIdentity();
            var userGuid = new Guid(userId);
            var getUserClaims = _xrmServiceConnection.XrmServiceContext.new_ContactClaimSet.Where(x => x.new_UserIdContactClaim.Id == userGuid).ToList();
            foreach (var claim in getUserClaims.Select(contactClaim => new Claim(contactClaim.new_ClaimType, contactClaim.new_ClaimValue)))
            {
                claims.AddClaim(claim);
            }

            return claims;

        }


        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(string userId)
        {
            var getUserClaims = _xrmServiceConnection.XrmServiceContext.new_ContactClaimSet.Where(x => x.new_UserIdContactClaim.Id.ToString() == userId).ToList();
            foreach (var contactClaim in getUserClaims)
            {
                _xrmServiceConnection.XrmServiceContext.Delete("new_ContactClaimSet", contactClaim.Id);
            }
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }


        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public void Insert(Claim userClaim, string userId)
        {
            var getUser =
                _xrmServiceConnection.XrmServiceContext.ContactSet.Where(x => x.Id.ToString() == userId)
                    .ToList()
                    .First();
            var contactClaim = new new_ContactClaim
            {
                new_ClaimType = userClaim.Type,
                new_ClaimValue = userClaim.Value,
                new_contact_new_contactclaim = getUser
            };
            _xrmServiceConnection.XrmServiceContext.AddObject(contactClaim);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public void Delete(XrmIdentityUser user, Claim claim)
        {
            var contactClaim = _xrmServiceConnection.XrmServiceContext.new_ContactClaimSet.Where(x => x.new_UserIdContactClaim.Id.ToString() == user.Id && x.new_ClaimValue == claim.Value && x.new_ClaimType == claim.Type).ToList().First();
            _xrmServiceConnection.XrmServiceContext.Delete("new_ContactClaimSet", contactClaim.Id);
            _xrmServiceConnection.XrmServiceContext.SaveChanges();
        }

    }
}
