using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_shop.Models.Repository;
using E_shop.Models;
using System.IO;
using MvcContrib.Pagination;

namespace E_shop.Controllers
{
    public class SubcategoryController : BaseController
    {
        private readonly ISubcategoryRepository subcategoryRepository;
        private readonly ISubcategoryProductRepository subcategoryproductRepository;
        private Error err;

        public SubcategoryController(ISubcategoryRepository subcategoryRepository, ISubcategoryProductRepository subcategoryproductRepository, Error err)
        {
            this.subcategoryRepository = subcategoryRepository;
            this.subcategoryproductRepository = subcategoryproductRepository;
            this.err = err;
        }

        public ViewResult Index(int? id)
        {
            if (id != null)
                return View(subcategoryRepository.List().Where(x => x.CategoryID == id));
            else
                return View(subcategoryRepository.List());
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ViewResult Details(int id)
        {
            if (err.Errors)
            {
                ViewData["ErrorSubcategoryDelete"] = "Error";
                err.Errors = false;
            }

            return View(subcategoryRepository.Find(id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Create(Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                subcategoryRepository.Insert(subcategory);
                subcategoryRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(subcategoryRepository.Find(id));
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var obj = subcategoryRepository.Find(id);

            if (obj == null) return View("NotFound");

            if (ModelState.IsValid)
            {
                UpdateModel(obj, collection);
                subcategoryRepository.Save();
                return RedirectToAction("Details", new { id = obj.ID });
            }
            else return View(obj);
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var x = subcategoryproductRepository.List().Where(s => s.SubcategoryID == id).Select(s => s.ID);

            if (x.Count() > 0)
            {
                err.Errors = true;
                return RedirectToAction("Details", new { id });
            }
            subcategoryRepository.Delete(id);
            subcategoryRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                subcategoryRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPost]
        public ActionResult UploadCatalogImage(HttpPostedFileBase imagefile, int objID)
        {
            var obj = subcategoryRepository.Find(objID);
            if (obj == null) return View("NotFound");

            try
            {
                if (imagefile != null)
                {
                    string strExtension = Path.GetExtension(imagefile.FileName);
                    string strSaveFileName = objID + strExtension;
                    string strSaveFullPath = Path.Combine(Server.MapPath(Url.Content("~/Content")), Resources.GlobalString.SUBCATEGORY_MINI_IMAGES_FOLDER,
                        strSaveFileName);

                    if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);

                    imagefile.ResizeAndSave(Constants.SUBCATEGORY_MINI_IMAGE_HEIGHT, Constants.SUBCATEGORY_MINI_IMAGE_WIDTH, strSaveFullPath);

                    obj.ImageExt = strExtension;

                    subcategoryRepository.Save();
                }
            }
            catch (Exception ex)
            {
                string strErrorMessage = ex.Message;
                if (ex.InnerException != null) strErrorMessage = string.Format("{0} --- {1}", strErrorMessage, ex.InnerException.Message);
                ViewBag.ErrorMessage = strErrorMessage;
                return RedirectToAction("ErrorPage", "Home");
            }

            return RedirectToAction("Details", new { id = obj.ID });
        }

    }
}

