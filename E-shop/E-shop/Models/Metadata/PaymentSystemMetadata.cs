using E_shop.Code;
using E_shop.Resource.Models.PaymentSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_shop.Models.Metadata
{
    public class PaymentSystemMetadata
    {
        [Required(ErrorMessageResourceType = typeof(PaymentSystemLoc), ErrorMessageResourceName = "NominalRequired")]
        [RegularExpression("[0-9]+", ErrorMessageResourceName = "NominalInvalid", ErrorMessageResourceType = typeof(PaymentSystemLoc))]
        [LocalizedDisplayName("Nominal", NameResourceType = typeof(PaymentSystemLoc))]
        [Range(0,50000)]
        public string Nominal { get; set; }

        [Required(ErrorMessageResourceType = typeof(PaymentSystemLoc), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(PaymentSystemLoc))]
        [LocalizedDisplayName("UserName", NameResourceType = typeof(PaymentSystemLoc))]
        public string UserName { get; set; }

        [RegularExpression("[0-9a-z-]+", ErrorMessageResourceName = "KeyCodeInvalid", ErrorMessageResourceType = typeof(PaymentSystemLoc))]
        [LocalizedDisplayName("KeyCode", NameResourceType = typeof(PaymentSystemLoc))]
        [StringLength(300, ErrorMessageResourceName = "KeyCodeInvalid", ErrorMessageResourceType = typeof(PaymentSystemLoc))]
        public string KeyCode { get; set; }
    }
}