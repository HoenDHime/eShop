using E_shop.Models;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_shop.Code
{
    public class ProductListContainerViewModel
    {
        public IPagination<Product> ProductPagedList { get; set; }
        public ProductFilterViewModel ProductFilterViewModel { get; set; }
    }
}