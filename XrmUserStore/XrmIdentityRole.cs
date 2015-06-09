using System;
using Microsoft.AspNet.Identity;

namespace XrmUserStore
{
    public class XrmIdentityRole : IRole
    {
        /// <summary>
        /// Default constructor for Role 
        /// </summary>
        public XrmIdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Constructor that takes names as argument 
        /// </summary>
        /// <param name="name"></param>
        public XrmIdentityRole(string name) : this()
        {
            Name = name;
        }

        public XrmIdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Role ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }
    }
}
