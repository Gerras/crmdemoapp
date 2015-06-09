using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xrm;

namespace CrmDemoApp.Models.ManageModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Business Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Display(Name = "Birthday")]
        public string Birthday { get; set; }


        public static ProfileViewModel InitializeViewModel(Contact contact)
        {
            return new ProfileViewModel
            {
                FullName = contact.FullName,
                Address = contact.Address1_Line1,
                Birthday = contact.BirthDate.ToString(),
                JobTitle = contact.JobTitle,
                Email = contact.EMailAddress1,
                PhoneNumber = contact.MobilePhone
            };
        }
    }
}