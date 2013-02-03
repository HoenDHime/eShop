using E_shop.Code;
using E_shop.Resource.Models.Subcategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_shop.Models.Metadata
{
    public class SubcategoryMetadata
    {
        [Required(ErrorMessageResourceType = typeof(SubcategoryLoc), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(SubcategoryLoc))]
        [LocalizedDisplayName("Name", NameResourceType = typeof(SubcategoryLoc))]
        public string NameRu { get; set; }

        [Required(ErrorMessageResourceType = typeof(SubcategoryLoc), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(SubcategoryLoc))]
        [LocalizedDisplayName("Name", NameResourceType = typeof(SubcategoryLoc))]
        public string NameEn { get; set; }

        //[Display(Name = "Категория")]
        [Required]
        [LocalizedDisplayName("Category", NameResourceType = typeof(SubcategoryLoc))]
        public string CategoryID { get; set; }


        
    }
}