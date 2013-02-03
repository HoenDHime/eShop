using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_shop.Models;
using E_shop.Models.Repository;
using System.IO;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;

namespace E_shop.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductRepository productRepository;
        private readonly ISubcategoryProductRepository subcategoryProductRepository;
        private Error err;

        public ProductController(IProductRepository productRepository, ISubcategoryProductRepository subcategoryProductRepository, Error err)
        {
            this.productRepository = productRepository;
            this.subcategoryProductRepository = subcategoryProductRepository;
            this.err = err;
        }

        #region Actions       

        // Показывает товары с использованием сортировки и выбранной подкатегории
        // Show products using sorting and selected subcategory
        public ViewResult Index(int? id, int? page, GridSortOptions gridSort)
        {
            var products = productRepository.List();

            if (id != null)
                products = subcategoryProductRepository.List().Where(x => x.Subcategory.ID == id).OrderBy(x => x.Subcategory.ID).Select(x => x.Product);

            if (gridSort.Column != null)
            {
                if (gridSort.Column == "Наименование" || gridSort.Column == "Name")
                    products = products.OrderBy(x => x.NameRu);
                else if (gridSort.Column == "Наличие скидки" || gridSort.Column == "Available Discounts")
                    products = products.OrderByDescending(s => s.AvailableDiscounts);
                else if (gridSort.Column == "Date")
                    products = products.OrderByDescending(s => s.Date);
                else if (gridSort.Column == "Цена" || gridSort.Column == "Price")
                    products = products.OrderBy(s => s.Price);
                else if (gridSort.Column == "По количеству продаж" || gridSort.Column == "The number of sales")           
                    products = products.OrderByDescending(prod => prod.InStock - prod.CurrentBalance);               
            }

            ViewData["sortGrid"] = gridSort;

            if (gridSort.Direction == SortDirection.Descending)
                return View(products.Reverse().ToList().AsPagination(page ?? 1, 6));
            else
                return View(products.ToList().AsPagination(page ?? 1, 6));
        }

        public ViewResult Details(int id)
        {
            if (err.Errors)
            {
                ViewData["ErrorProductDelete"] = "Error";
                err.Errors = false;
            }

            return View(productRepository.Find(id));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    productRepository.Insert(product);
                    productRepository.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage","Home");
            }
        }

        public ActionResult Edit(int id)
        {
            return View(productRepository.Find(id));
        }

        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var obj = productRepository.Find(id);
            try
            {
                if (obj == null) return View("NotFound");

                if (ModelState.IsValid)
                {
                    ChangeFormCollectionValues(obj, collection);
                    UpdateModel(obj, collection);
                    productRepository.Save();
                    return RedirectToAction("Details", new { id = obj.ID });
                }
                else return View(obj);
            }
            catch (Exception) { return View(obj); }
        }

        // Удаление невозможно если есть наследники
        // Can not be removed if there are heirs
        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Delete(int id)
        {
            var x = subcategoryProductRepository.List().Where(s => s.ProductID == id).Select(s => s.ID);

            if (x.Count() > 0)
            {
                err.Errors = true;
                return RedirectToAction("Details", new { id });
            }
            productRepository.Delete(id);
            productRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        // Добавление возможности вводить как точку так и запятую
        // Adding the ability to enter a point or a comma
        protected void ChangeFormCollectionValues(Product obj, FormCollection collection)
        {
            collection["Price"] = collection["Price"].Replace(" ", "");
            collection["Price"] = collection["Price"].Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            collection["Price"] = collection["Price"].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

            collection["PurchasePrice"] = collection["PurchasePrice"].Replace(" ", "");
            collection["PurchasePrice"] = collection["PurchasePrice"].Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            collection["PurchasePrice"] = collection["PurchasePrice"].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
        }

        // Обработка загрузки картинки
        // Processing load pictures
        [HttpPost]
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult UploadImage(HttpPostedFileBase imagefile, int objID)
        {
            // Получаем объект, для которого загружаем картинку
            // We get the object to load the image
            var obj = productRepository.Find(objID);
            if (obj == null) return View("NotFound");

            try
            {
                if (imagefile != null)
                {
                    // Определяем название и полный путь полноразмерной картинки и миниатюры
                    // Determine the name and full path of a full-size images and thumbnails
                    string strExtension = Path.GetExtension(imagefile.FileName);
                    string strSaveFileName = objID + strExtension;
                    string strSaveFullPath = Path.Combine(Server.MapPath(Url.Content("~/Content")),
                        Resources.GlobalString.PRODUCT_IMAGES_FOLDER, strSaveFileName);
                    string strSaveMiniFullPath = Path.Combine(Server.MapPath(Url.Content("~/Content")),
                        Resources.GlobalString.PRODUCT_IMAGES_FOLDER, Resources.GlobalString.PRODUCT_IMAGES_MINI_FOLDER, strSaveFileName);

                    // Если файлы с такими названиями уже имеются, удаляем их
                    // If the files with names already exist, delete them
                    if (System.IO.File.Exists(strSaveFullPath))
                        System.IO.File.Delete(strSaveFullPath);

                    if (System.IO.File.Exists(strSaveMiniFullPath))
                        System.IO.File.Delete(strSaveMiniFullPath);

                    // Сохраняем полную картинку и миниатюру
                    // Retain full image and a thumbnail
                    imagefile.ResizeAndSave(Constants.PRODUCT_IMAGE_HEIGHT,
                        Constants.PRODUCT_IMAGE_WIDTH, strSaveFullPath);
                    imagefile.ResizeAndSave(Constants.PRODUCT_IMAGE_MINI_HEIGHT,
                        Constants.PRODUCT_IMAGE_MINI_WIDTH, strSaveMiniFullPath);

                    // Расширение файла записываем в базу данных в поле ImageExt
                    // The file extension is written to the database in the ImageExt
                    obj.ImageExt = strExtension;
                    productRepository.Save();
                }
            }
            catch (Exception ex)
            {
                string strErrorMessage = ex.Message;
                if (ex.InnerException != null) strErrorMessage = string.Format("{0} --- {1}",
                    strErrorMessage, ex.InnerException.Message);
                ViewBag.ErrorMessage = strErrorMessage;
                return RedirectToAction("ErrorPage", "Home");
            }

            return RedirectToAction("Details", new { id = obj.ID });
        }
    }
}

