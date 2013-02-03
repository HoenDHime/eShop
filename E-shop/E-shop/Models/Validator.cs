using E_shop.Models.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_shop.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    { }

    [MetadataType(typeof(SubcategoryMetadata))]
    public partial class Subcategory
    { }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    { }

    [MetadataType(typeof(PaymentSystemMetadata))]
    public partial class PaymentSystem
    { }

    [MetadataType(typeof(OrderStatusMetadata))]
    public partial class OrderStatus
    { }
}