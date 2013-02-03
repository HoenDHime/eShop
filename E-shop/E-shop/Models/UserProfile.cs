using E_shop.Code;
using E_shop.Resource.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace E_shop.Models
{
    public class UserProfile
    {
        private ProfileBase profile = null;

        public UserProfile(string username)
        {
            profile = ProfileBase.Create(username);           
        }

        [Required(ErrorMessageResourceType = typeof(UserProf), ErrorMessageResourceName = "FirstNameRequired")]
        [LocalizedDisplayName("FirstName", NameResourceType = typeof(UserProf))]
        [StringLength(50, ErrorMessageResourceName = "Invalid50", ErrorMessageResourceType = typeof(UserProf))]
        public string FirstName
        {
            get
            {
                if (profile == null)
                    return string.Empty;
                else return profile[Resources.GlobalString.PROFILE_FIRST_NAME] as string;
            }
            set
            {
                if (profile != null)
                    profile[Resources.GlobalString.PROFILE_FIRST_NAME] = value;
            }
        }

        [Required(ErrorMessageResourceType = typeof(UserProf), ErrorMessageResourceName = "LastNameRequired")]
        [LocalizedDisplayName("LastName", NameResourceType = typeof(UserProf))]
        [StringLength(50, ErrorMessageResourceName = "Invalid50", ErrorMessageResourceType = typeof(UserProf))]
        public string LastName
        {
            get
            {
                if (profile == null) return string.Empty;
                else return profile[Resources.GlobalString.PROFILE_LAST_NAME] as string;
            }
            set { if (profile != null) { profile[Resources.GlobalString.PROFILE_LAST_NAME] = value; } }
        }

        [Required(ErrorMessageResourceType = typeof(UserProf), ErrorMessageResourceName = "PhoneRequired")]
        [LocalizedDisplayName("Phone", NameResourceType = typeof(UserProf))]
        [StringLength(50, ErrorMessageResourceName = "Invalid50", ErrorMessageResourceType = typeof(UserProf))]
        public string Phone
        {
            get
            {
                if (profile == null) return string.Empty;
                else return profile[Resources.GlobalString.PROFILE_PHONE] as string;
            }
            set { if (profile != null) { profile[Resources.GlobalString.PROFILE_PHONE] = value; } }
        }

        [Required(ErrorMessageResourceType = typeof(UserProf), ErrorMessageResourceName = "AddressRequired")]
        [LocalizedDisplayName("Address", NameResourceType = typeof(UserProf))]
        [StringLength(1000, ErrorMessageResourceName = "Invalid1000", ErrorMessageResourceType = typeof(UserProf))]
        public string Address
        {
            get
            {
                if (profile == null) return string.Empty;
                else return profile[Resources.GlobalString.PROFILE_ADDRESS] as string;
            }
            set { if (profile != null) { profile[Resources.GlobalString.PROFILE_ADDRESS] = value; } }
        }

        public decimal? Money 
        {
            get
            {
                if (profile == null) return 0;
                else return profile[Resources.GlobalString.PROFILE_MONEY] as decimal?;
            }
            set { if (profile != null) { profile[Resources.GlobalString.PROFILE_MONEY] = value; } }
        }

        public void Save()
        {
            if (profile != null) profile.Save();
        }
    }
}