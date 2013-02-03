using E_shop.Code;
using E_shop.Resource.Models.OrderStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_shop.Models.Metadata
{
    public class OrderStatusMetadata
    {
        [Required(ErrorMessageResourceType = typeof(OrderStatusLoc), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(OrderStatusLoc))]
        [LocalizedDisplayName("Name", NameResourceType = typeof(OrderStatusLoc))]
        public string NameRu { get; set; }

        [Required(ErrorMessageResourceType = typeof(OrderStatusLoc), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(OrderStatusLoc))]
        [LocalizedDisplayName("Name", NameResourceType = typeof(OrderStatusLoc))]
        public string NameEn { get; set; }
    }
}