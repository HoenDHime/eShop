using E_shop.Models;
using E_shop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using E_shop.Code;
using MvcContrib.Sorting;

namespace E_shop.Controllers
{
    //Поиск товаров
    // Search products
    public class SearchController : BaseController
    {
        private readonly ISubcategoryProductRepository subcategoryProductRepository;
        private readonly IProductRepository productRepository;
        ProductFilterViewModel productFilterViewModel;

        public SearchController(ISubcategoryProductRepository subcategoryProductRepository, IProductRepository productRepository, 
            ProductFilterViewModel productFilterViewModel)
        {
            this.subcategoryProductRepository = subcategoryProductRepository;
            this.productRepository = productRepository;
            this.productFilterViewModel = productFilterViewModel;
        }

        // Показ всех товаров если не выбранны критерии поиска и взаимная фильтрация всех выбранных фильтров 
        // Showing all the goods unless the search criteria and selection at the mutual screening of all selected filters
        public ActionResult Index(int? subcategoryID, string Name, string DateFromFirst, string DateToLast, int? page) // DateTime?
        {
            var subcaprod = subcategoryProductRepository.List();
            var product = productRepository.List();
            try
            {
                string cultureName = null;
                HttpCookie cultureCookie = Request.Cookies["_culture"];
                if (cultureCookie != null) { cultureName = cultureCookie.Value; }
                else { cultureName = Request.UserLanguages[0]; }
                cultureName = E_shop.Utility.CultureHelper.GetImplementedCulture(cultureName);

                if (subcategoryID.HasValue)
                {
                    product = subcaprod.Where(s => s.Subcategory.ID == subcategoryID).Select(x => x.Product);
                }

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    product = product.Where(x => x.NameRu.ToLower().Contains(Name.ToLower()));
                }

                productFilterViewModel.SelectedSubcategoryID = subcategoryID ?? -1;

                if (!cultureName.Equals("en"))
                    productFilterViewModel.FillRu();
                else productFilterViewModel.FillEn();

                if (!string.IsNullOrWhiteSpace(DateFromFirst))
                {
                    var DFF = DateTime.Parse(DateFromFirst);
                    DFF = DFF.AddDays(-1);

                    List<Product> result = new List<Product>();
                    foreach (var i in product)
                    {
                        var LeftBorderYear = new DateTime(DFF.Year, DFF.Month, DFF.Day);
                        var LBY = DateTime.Compare(LeftBorderYear, (DateTime)i.Date);

                        DateTime RightBorderYear;

                        if (!string.IsNullOrWhiteSpace(DateToLast))
                        {
                            var DTL = DateTime.Parse(DateToLast);
                            DTL = DTL.AddDays(1);

                            RightBorderYear = new DateTime(DTL.Year, DTL.Month, DTL.Day);
                        }
                        else
                            RightBorderYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                        var RBY = DateTime.Compare(RightBorderYear, (DateTime)i.Date);

                        if (LBY == -1 && RBY == 1) result.Add(i);
                    }
                    product = result;
                }
            }
            catch (Exception) { }

            var productListContainer = new ProductListContainerViewModel
            {
                ProductPagedList = product.OrderBy(x => x.ID).AsPagination(page ?? 1, 6),
                ProductFilterViewModel = productFilterViewModel,
            };

            return View(productListContainer);
        }
    }
}
