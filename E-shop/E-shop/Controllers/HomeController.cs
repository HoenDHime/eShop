using E_shop.Models;
using E_shop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Pagination;
using E_shop.Utility;
using E_shop.Code;

namespace E_shop.Controllers
{
    //[OutputCache(Duration = 60, VaryByCustom = "culture")] //OutputCacheAttribute
    public class HomeController : BaseController //: Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public ViewResult Index(int? page) 
        {
            return View();
        }

        public PartialViewResult Tree() 
        {
            return PartialView(categoryRepository.List());
        }      

        public PartialViewResult ProductOrderByDate(int? page)
        {
            return PartialView(productRepository.List().OrderByDescending(prod => prod.Date).AsPagination(page ?? 1, 6)); 
        }

        public PartialViewResult ProductAvailableDiscount(int? page) 
        {
            return PartialView(productRepository.List().Where(x => x.AvailableDiscounts).AsPagination(page ?? 1, 6)); 
        }

        public PartialViewResult ProductOrderByBestSelling(int? page) 
        {
            return PartialView(productRepository.List().OrderByDescending(prod => prod.InStock - prod.CurrentBalance).AsPagination(page ?? 1, 6));
        }

        public ViewResult ErrorPage() 
        {
            return View();
        }

        //Устанавливаем выбранный язык
        //Set the selected language
        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);

            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {

                cookie = new HttpCookie("_culture");
                cookie.HttpOnly = false; // Not accessible by JS.
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }
    }
}
