using E_shop.Code;
using E_shop.Resource.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace E_shop.Models
{

    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.AccountRes), ErrorMessageResourceName = "OldPasswordRequired")]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("OldPassword", NameResourceType = typeof(Names))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.AccountRes), ErrorMessageResourceName = "NewPasswordRequired")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("NewPassword", NameResourceType = typeof(Names))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("ConfirmPassword", NameResourceType = typeof(Names))]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordNewOld", ErrorMessageResourceType = typeof(Resources.AccountRes))]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.AccountRes), ErrorMessageResourceName = "UserNameRequired")]
        [LocalizedDisplayName("UserName", NameResourceType = typeof(Names))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.AccountRes), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("Password", NameResourceType = typeof(Names))]
        public string Password { get; set; }

        [LocalizedDisplayName("RememberMe", NameResourceType = typeof(Names))]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.AccountRes), ErrorMessageResourceName = "UserNameRequired")]
        [LocalizedDisplayName("UserName", NameResourceType = typeof(Names))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.AccountRes), ErrorMessageResourceName = "EmailRequired")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessageResourceType = typeof(Resources.AccountRes),
            ErrorMessageResourceName = "EmailInvalid")]
        [DataType(DataType.EmailAddress)]
        [LocalizedDisplayName("Email", NameResourceType = typeof(Names))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.AccountRes), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100, ErrorMessageResourceName = "PasswordInvalid", MinimumLength = 6 ,ErrorMessageResourceType = typeof(Resources.AccountRes))]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("Password", NameResourceType = typeof(Names))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("Password", ErrorMessageResourceName = "ConfirmPasswordInvalid", ErrorMessageResourceType = typeof(Resources.AccountRes))]
        [LocalizedDisplayName("ConfirmPassword", NameResourceType = typeof(Names))]
        public string ConfirmPassword { get; set; }
    }
}
