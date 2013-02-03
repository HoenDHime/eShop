using E_shop.Models;
using E_shop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_shop.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class OrderStatusController : BaseController
    {
        private readonly IOrderStatusRepository orderstatusRepository;

        public OrderStatusController(IOrderStatusRepository orderstatusRepository)
        {
            this.orderstatusRepository = orderstatusRepository;
        }
        
        public ViewResult Index()
        {
            return View(orderstatusRepository.List());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(OrderStatus orderstatus)
        {
            if (ModelState.IsValid)
            {
                orderstatusRepository.Insert(orderstatus);
                orderstatusRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(orderstatusRepository.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var obj = orderstatusRepository.Find(id);          

            if (obj == null) return View("NotFound");

            if (ModelState.IsValid)
            {
                UpdateModel(obj, collection);
                orderstatusRepository.Save();
                return RedirectToAction("Index");
            }
            else return View(obj);
        }

        public ActionResult Delete(int id)
        {
            orderstatusRepository.Delete(id);
            orderstatusRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                orderstatusRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
