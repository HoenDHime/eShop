using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_shop.Models;
using E_shop.Models.Repository;
using System.IO;

namespace E_shop.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ISubcategoryRepository subcategoryRepository;
        private Error err;

        public CategoryController(ICategoryRepository categoryRepository, ISubcategoryRepository subcategoryRepository, Error err)
        {
            this.categoryRepository = categoryRepository;
            this.subcategoryRepository = subcategoryRepository;
            this.err = err;
        }

        #region Actions

        public ViewResult Index()
        {
            return View(categoryRepository.List());
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ViewResult Details(int id)
        {
            //if (err.Errors)
            //{
            //    ViewData["ErrorCategoryDelete"] = "Error";
            //    err.Errors = false;
            //}

            return View(categoryRepository.Find(id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Insert(category);
                categoryRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(categoryRepository.Find(id));
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var obj = categoryRepository.Find(id);          

            if (obj == null) return View("NotFound");

            if (ModelState.IsValid)
            {
                UpdateModel(obj, collection);
                categoryRepository.Save();
                return RedirectToAction("Details", new { id = obj.ID });
            }
            else return View(obj);
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Delete(int id)
        {
            var x = subcategoryRepository.List().Where(s => s.CategoryID == id).Select(s=>s.ID);

            if (x.Count() > 0)
            {
                err.Errors = true;
                return RedirectToAction("Details", new { id });
            }
            categoryRepository.Delete(id);
            categoryRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                categoryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPost]
        public ActionResult UploadCatalogImage(HttpPostedFileBase imagefile, int objID)
        {
            // Получаем объект, для которого загружаем картинку
            // We get the object to load the image
            var obj = categoryRepository.Find(objID);
            if (obj == null) return View("NotFound");

            try
            {
                if (imagefile != null)
                {
                    // Определяем название конечного графического файла вместе с полным путём.
                    // Название файла должно быть такое же, как ID объекта. Это гарантирует уникальность названия.
                    // Расширение должно быть такое же, как расширение у исходного графического файла.
                    // Determine the final image file name with full path.                     
                    // The file name must be the same as the ID of the object. This ensures the uniqueness of the name.                     
                    // Extension must be the same as the expansion of the original image file.
                    string strExtension = Path.GetExtension(imagefile.FileName);
                    string strSaveFileName = objID + strExtension;
                    string strSaveFullPath = Path.Combine(Server.MapPath(Url.Content("~/Content")),
                        Resources.GlobalString.CATEGORY_MINI_IMAGES_FOLDER, strSaveFileName);

                    // Если файл с таким названием имеется, удаляем его.
                    //If a file by that name exists, delete it.
                    if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);

                    // Сохраняем картинку, изменив её размеры.
                    //Save the image by changing its size.
                    imagefile.ResizeAndSave(Constants.CATEGORY_MINI_IMAGE_HEIGHT, Constants.CATEGORY_MINI_IMAGE_WIDTH,
                        strSaveFullPath);

                    // Расширение файла записываем в базу данных в поле ImageExt.
                    //The file extension is written to the database in the ImageExt.
                    obj.ImageExt = strExtension;

                    categoryRepository.Save();
                }
            }
            catch (Exception ex)
            {
                string strErrorMessage = ex.Message;
                if (ex.InnerException != null) strErrorMessage = string.Format("{0} --- {1}", strErrorMessage,
                    ex.InnerException.Message);
                ViewBag.ErrorMessage = strErrorMessage;
                return RedirectToAction("ErrorPage", "Home");
            }

            return RedirectToAction("Details", new { id=obj.ID});
        }
    }
}

