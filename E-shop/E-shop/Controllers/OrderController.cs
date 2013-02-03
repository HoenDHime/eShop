using E_shop.Models;
using E_shop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcContrib.Pagination;
using E_shop.Service;

namespace E_shop.Controllers
{  
    public class OrderController : BaseController
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderStatusRepository orderStatusRepository;
        private readonly IProductRepository productRepository;
        private readonly ViewOrders order;
        private readonly ViewOrderProfit profit;
        private Filter filter;
        private readonly IOrderItemRepository orderitemRepository;

        public OrderController(IOrderRepository orderRepository, IOrderStatusRepository orderStatusRepository, IProductRepository productRepository,
            ViewOrders order, ViewOrderProfit profit, Filter filter, IOrderItemRepository orderitemRepository)
        {
            this.orderRepository = orderRepository;
            this.orderStatusRepository = orderStatusRepository;
            this.productRepository = productRepository;
            this.order = order;
            this.profit = profit;
            this.filter=filter;
            this.orderitemRepository = orderitemRepository;
        }

        #region CRUD View

        // Просмотр заказов + выполнение фильтрации
        // Views orders + filtering implementation
        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ViewResult Index(string Filters, int? page)
        {
            var orders = orderRepository.List();

            if (Filters != null)
            {
                filter.Filtering = Filters;
                ViewData["Filters"] = Filters;
            }

            if (filter.Filtering != null && Filters == null)
                ViewData["Filters"] = filter.Filtering;

            if (ViewData["Filters"] != null)
            {
                switch (ViewData["Filters"].ToString())
                {
                    case "1": //периоду - прошедший месяц  period - the last month
                        orders = order.Period(ViewData["Filters"].ToString(), orders);
                        break;
                    case "2": //периоду - текущий месяц period - the current month
                        orders = order.Period(ViewData["Filters"].ToString(), orders);
                        break;
                    case "3": //периоду - текущий год period - current year
                        orders = order.Period(ViewData["Filters"].ToString(), orders);
                        break;

                    case "4": //прибыли с периода - прошедший месяц profit from the period - the last month
                        ViewData["profit"] = profit.ProfitFromThePeriod(Filters);
                        break;
                    case "5": //прибыли с периода - текущий месяц profit from the period - current month
                        ViewData["profit"] = profit.ProfitFromThePeriod(Filters);
                        break;
                    case "6": //прибыли с периода - текущий год profit from the period - current year
                        ViewData["profit"] = profit.ProfitFromThePeriod(Filters);
                        break;
                }
            }
            return View(orders.ToList().OrderByDescending(x=>x.Date).AsPagination(page ?? 1, 10));
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ViewResult Details(int id)
        {
            return View(orderRepository.Find(id));
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        public ActionResult Delete(int id)
        {
            return View(orderRepository.Find(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            orderRepository.Delete(id);
            orderRepository.Save();

            return RedirectToAction("Index");
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                orderRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        private string OrderStatusForEmailRu { get;set; }
        private string OrderStatusForEmailEn { get; set; }

        // Изменение статуса товара
        // Change of Status
        public ActionResult ChangeStatus(int orderID, int statusID)
        {
           var orderStatus = orderStatusRepository.Find(statusID);

            if (orderStatus != null)
            {
                var order = orderRepository.Find(orderID);

                if (order != null)
                {
                    string cultureName = null;
                    HttpCookie cultureCookie = Request.Cookies["_culture"];
                    if (cultureCookie != null) { cultureName = cultureCookie.Value; }
                    else { cultureName = Request.UserLanguages[0]; }
                    cultureName = E_shop.Utility.CultureHelper.GetImplementedCulture(cultureName);

                    order.OrderStatusRu = orderStatus.NameRu;
                    order.OrderStatusEn = orderStatus.NameEn;
                    orderRepository.Save();
                    OrderStatusForEmailRu = order.OrderStatusRu;
                    OrderStatusForEmailEn = order.OrderStatusEn;

                    //-------------------------------------------------------------
                    // Снить деньги, когда статус заказа стал - доставлен. А если у пользователя недостаточно денег на счету? 
                    //var x = order.PaymentSys;
                    //if (order.PaymentSys.Equals("Internal payment system") || order.PaymentSys.Equals("Внутреняя система оплаты"))
                    //{
                    //    UserProfile profile = new UserProfile(order.UserName);
                    //    UserProfile profileAdmin = new UserProfile(Resources.GlobalString.ADMIN_NAME);
                    //    var orderItem = orderitemRepository.Find(orderID);

                    //    //if (profile.Money > (decimal)product.Price * cart[key])
                    //   if(orderStatus.Name.Equals("Доставлен/Оплачен"))
                    //    {
                    //        //profile.Money = profile.Money - (decimal)product.Price * cart[key];
                    //        profile.Money = profile.Money - orderItem.Price * orderItem.Number;
                    //        profile.Save();

                    //        //profileAdmin.Money = profileAdmin.Money + (0.5m * (orderItem.Price - orderItem. product.PurchasePrice) * cart[key]);

                    ////        //written to the database profit
                    ////        profit = (0.5m * (decimal)(product.Price - product.PurchasePrice) * cart[key]);
                    //    }
                    ////    else return RedirectToAction("Index");
                    //}
                    //else
                    //{
                    //    profileAdmin.Money = profileAdmin.Money + (0.1m * (decimal)(product.Price - product.PurchasePrice));
                    //}

                    //------------------------------------------------------------
                    Mail(order.UserName, order.Email);
                }
            }

            return RedirectToAction("Details", new { id = orderID }); 
        }

        // Отсылка письма при смене статуса товара
        //Sending letters by changing the status of the products
        [HttpPost]
        public void Mail(string UserName, string Email)
        {
            MailMessage message = new MailMessage();
            if (UserName != null)
            {
                var m = Membership.GetUser(UserName);
                message.To.Add(m.Email); 
            }
            else message.To.Add(Email);
            message.Subject = Resources.GlobalString.OrderStatusRu + " " + Resources.GlobalString.OrderStatusEn;
            message.From = new MailAddress(Resources.GlobalString.userSmtp);
            message.Body = Resources.GlobalString.OrderStatusRu + " - " + OrderStatusForEmailRu + "<br />" + "<br />" + Resources.GlobalString.DoNotRespondRu + "<br /><br />"
                + Resources.GlobalString.OrderStatusEn + " - " + OrderStatusForEmailEn + "<br />" + "<br />" + Resources.GlobalString.DoNotRespondEn;
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(Resources.GlobalString.hostSmtp);
            if (!String.IsNullOrEmpty(Resources.GlobalString.userSmtp))
            {
                smtp.Credentials = new System.Net.NetworkCredential(Resources.GlobalString.userSmtp, Resources.GlobalString.passSmtp);
            }
            smtp.Send(message);
            message.Dispose();
        }

        public ViewResult BestSellingProducts(int? page) 
        {
            return View(productRepository.List().OrderByDescending(prod => prod.InStock - prod.CurrentBalance).AsPagination(page ?? 1, 10));
        }

        //Отмена заказа пользователем на личной странице
        //Cancellation by the user on a personal page
        public ActionResult Cancellation(int id, int? orderID)
        {
            var orderStatus = orderStatusRepository.Find(3);

            if (orderID.HasValue)
            {
                var order = orderRepository.Find(orderID.Value);

                if (order != null)
                {
                    order.OrderStatusRu = orderStatus.NameRu;
                    order.OrderStatusEn = orderStatus.NameEn;
                    orderRepository.Save();
                    OrderStatusForEmailRu = order.OrderStatusRu;
                    OrderStatusForEmailEn = order.OrderStatusEn;

                    Mail(order.UserName, order.Email);
                }

                return RedirectToAction("Index","Personal");
            }


            return View(orderRepository.Find(id));
        }
    }
}
