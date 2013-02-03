using E_shop.Models;
using E_shop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;

namespace E_shop.Controllers
{
    public class SubcategoryProductController : BaseController
    {
        private readonly IProductRepository productRepository;
        private readonly ISubcategoryProductRepository subcategoryProductRepository;

        public SubcategoryProductController(IProductRepository productRepository, ISubcategoryProductRepository subcategoryProductRepository)
        {
            this.productRepository = productRepository;
            this.subcategoryProductRepository = subcategoryProductRepository;
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult DeleteExpress(int id, int IdP)
        {
            var obj = subcategoryProductRepository.Find(id);
            if (obj == null) return View("NotFound");

            subcategoryProductRepository.Delete(id);
            subcategoryProductRepository.Save();

            return RedirectToAction("Details", "Product", new { id = IdP });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                subcategoryProductRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Add(int? SubcategoryID, int ProductID)
        {
            var product = productRepository.Find(ProductID);
            var subcategory = subcategoryProductRepository.Find(ProductID);

            if (SubcategoryID.HasValue && product.SubcategoryProducts.Where(item => item.SubcategoryID == SubcategoryID.Value).Count() == 0)
            {
                SubcategoryProduct obj = new SubcategoryProduct();
                obj.SubcategoryID = SubcategoryID.Value;
                obj.ProductID = ProductID;
                subcategoryProductRepository.Insert(obj);
                subcategoryProductRepository.Save();
            }

            return RedirectToAction("Details", "Product", new { id = ProductID });
        }       
    }
}
