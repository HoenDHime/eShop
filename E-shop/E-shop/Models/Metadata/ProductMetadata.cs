using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using E_shop.Code;
using E_shop.Resource.Models.Products;

namespace E_shop.Models.Metadata
{
    public class ProductMetadata
    {       
        [Required(ErrorMessageResourceType = typeof(ProductLoc), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(ProductLoc))]
        [LocalizedDisplayName("Name", NameResourceType = typeof(ProductLoc))]
        public string NameRu { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductLoc), ErrorMessageResourceName = "NameRequired")]
        [StringLength(50, ErrorMessageResourceName = "NameInvalid", ErrorMessageResourceType = typeof(ProductLoc))]
        [LocalizedDisplayName("Name", NameResourceType = typeof(ProductLoc))]
        public string NameEn { get; set; }

        [LocalizedDisplayName("Description", NameResourceType = typeof(ProductLoc))]
        public string DescriptionRu { get; set; }

        [LocalizedDisplayName("Description", NameResourceType = typeof(ProductLoc))]
        public string DescriptionEn { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductLoc), ErrorMessageResourceName = "PriceRequired")]
        //[RegularExpression("[0-9]+", ErrorMessageResourceName = "PriceInvalid", ErrorMessageResourceType = typeof(ProductLoc))]
        [LocalizedDisplayName("Price", NameResourceType = typeof(ProductLoc))]
        //[Range(0.0,50000.0)]
        //[RangeAttribute(0.0,50000.0)]
        //[Range(typeof(decimal), "0", "50000")]
        //[Range(typeof(decimal), "1", "10000", ErrorMessage = "Between 1.0 and 10000.0, please!")]
        //[Range(typeof(decimal), "0.00", "50000.00")]
        public string Price { get; set; }

        [LocalizedDisplayName("AvailableDiscounts", NameResourceType = typeof(ProductLoc))]
        public string AvailableDiscounts { get; set; }
        
        [RegularExpression("[0-9]+", ErrorMessageResourceName = "DiscountInvalid", ErrorMessageResourceType = typeof(ProductLoc))]
        [LocalizedDisplayName("Discount", NameResourceType = typeof(ProductLoc))]
        public string Discount { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductLoc), ErrorMessageResourceName = "DateRequired")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        //[RegularExpression(@"^((0?[1-9]|[12][1-9]|3[01])\.(0?[13578]|1[02])\.20[0-9]{2}|(0?[1-9]|[12][1-9]|30)\.(0?[13456789]|1[012])\.20[0-9]{2}|(0?[1-9]|1[1-9]|2[0-8])\.(0?[123456789]|1[012])\.20[0-9]{2}|(0?[1-9]|[12][1-9])\.(0?[123456789]|1[012])\.20(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96))$")]
        //[RegularExpression(@"(29\.02\.(((([02468][048])|([13579][26]))00)|(\d{2}(([02468][48])|([2468][048])|([13579][26])))))|(((((0[1-9])|(1\d)|(2[0-8]))\.02)|(((0[1-9])|(1\d)|(30))\.((04)|(06)|(09)|(11)))|(((0[1-9])|(1\d)|([3][0-1]))\.((01)|(03)|(05)|(07)|(08)|(10)|(12))))\.\d{4})")]
        //[DataType(DataType.DateTime)]
        //[StringLength(10, ErrorMessage = "Было превышено допустимое количество символов на ввод. Только цифры, точки")]
        //[DateRange("2010/12/01", "2010/12/16")]
        //[RegularExpression(@"^(((0[1-9]|[12]\d|3[01])\.(0[13578]|1[02])\.((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\.(0[13456789]|1[012])\.((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\.02\.((19|[2-9]\d)\d{2}))|(29\.02\.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$")]
        //[Display(Name = "Дата добавления")]
        [LocalizedDisplayName("Date", NameResourceType = typeof(ProductLoc))]
        public string Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductLoc), ErrorMessageResourceName = "PurchasePriceRequired")]
        //[RegularExpression("[0-9]+", ErrorMessageResourceName = "PriceInvalid", ErrorMessageResourceType = typeof(ProductLoc))]
        [LocalizedDisplayName("PurchasePrice", NameResourceType = typeof(ProductLoc))]
        public string PurchasePrice { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductLoc), ErrorMessageResourceName = "InStockRequired")]
        [RegularExpression("[0-9]+", ErrorMessageResourceName = "DiscountInvalid", ErrorMessageResourceType = typeof(ProductLoc))]
        [LocalizedDisplayName("InStock", NameResourceType = typeof(ProductLoc))]
        public string InStock { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ProductLoc), ErrorMessageResourceName = "CurrentBalanceRequired")]
        [RegularExpression("[0-9]+", ErrorMessageResourceName = "DiscountInvalid", ErrorMessageResourceType = typeof(ProductLoc))]
        [LocalizedDisplayName("CurrentBalance", NameResourceType = typeof(ProductLoc))]
        public string CurrentBalance { get; set; }

            
    }
}