using E_shop.Code;
using E_shop.Resource.Models.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_shop.Models.Metadata
{
    public class CategoryMetadata
    {
        [Required(ErrorMessageResourceType = typeof(CategoryLocal), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(CategoryLocal))]
        [LocalizedDisplayName("Name", NameResourceType = typeof(CategoryLocal))]
        public string NameRu { get; set; }

        [Required(ErrorMessageResourceType = typeof(CategoryLocal), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(CategoryLocal))]
        [LocalizedDisplayName("Name", NameResourceType = typeof(CategoryLocal))]
        public string NameEn { get; set; }
    }
}