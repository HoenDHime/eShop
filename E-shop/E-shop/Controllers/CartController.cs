using E_shop.Models;
using E_shop.Models.Repository;
using E_shop.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace E_shop.Controllers
{
    public class CartController : BaseController
    {
        OrderItem orderItem = new OrderItem();
        Order order = new Order();
        OrderFilling of;
        OrderItemFilling oif;

        private readonly IProductRepository productRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IOrderStatusRepository orderStatusRepository;
        private readonly IOrderRepository orderRepository;

        public CartController(IProductRepository productRepository, IOrderItemRepository orderItemRepository, IOrderStatusRepository orderStatusRepository,
            IOrderRepository orderRepository, OrderFilling of, OrderItemFilling oif)
        {
            this.productRepository = productRepository;
            this.orderItemRepository = orderItemRepository;
            this.orderStatusRepository = orderStatusRepository;
            this.orderRepository = orderRepository;
            this.of = of;
            this.oif = oif;
        }

        // Открытие страницы корзины
        public ActionResult Index()
        {
            return View(Session[Resources.GlobalString.SESSION_CART] as Dictionary<int, int>);
        }

        // Добавление товара в корзину
        //Adding product to cart
        public ActionResult Add(int id)
        {
            Dictionary<int, int> cart = Session[Resources.GlobalString.SESSION_CART] as Dictionary<int, int>;
            if (cart == null) cart = new Dictionary<int, int>();

            if (cart.ContainsKey(id)) cart[id] = cart[id] + 1;
            else cart[id] = 1;

            Session[Resources.GlobalString.SESSION_CART] = cart;

            return RedirectToAction("Index");
        }

        // Изменение количества товара
        // Changing the number of products
        public ActionResult ChangeValue(int id, string number)
        {
            int iNumber = 0;

            if (int.TryParse(number, out iNumber) && iNumber > 0)
            {
                Dictionary<int, int> cart = Session[Resources.GlobalString.SESSION_CART] as Dictionary<int, int>;
                if (cart != null && cart.ContainsKey(id)) cart[id] = iNumber;
                Session[Resources.GlobalString.SESSION_CART] = cart;
            }

            return RedirectToAction("Index");
        }

        // Изменение профиля
        //Edit profile
        [HttpPost]
        public ActionResult ChangeValueProfile(string LastName, string FirstName, string Phone, string Address)
        {
            UserProfile profile = new UserProfile(User.Identity.Name);
            Dictionary<int, int> cart = Session[Resources.GlobalString.SESSION_CART] as Dictionary<int, int>;
            if (cart != null)
            {
                profile.LastName = LastName;
                profile.FirstName = FirstName;
                profile.Phone = Phone;
                profile.Address = Address;
                profile.Save();
            }
            return RedirectToAction("Index");
        }

        // Удаление товара из корзины
        // Remove product from cart
        public ActionResult Delete(int id)
        {
            Dictionary<int, int> cart = Session[Resources.GlobalString.SESSION_CART] as Dictionary<int, int>;
            if (cart != null && cart.ContainsKey(id)) cart.Remove(id);
            Session[Resources.GlobalString.SESSION_CART] = cart;

            return RedirectToAction("Index");
        }

        protected int OrderID { get; set; }
 
        // Сохранение заказа
        // Save order
        [HttpPost]
        public ActionResult Save(string LastName, string FirstName, string Phone, string Address, string Email, string PaymentSys)
        {
            UserProfile profile = new UserProfile(User.Identity.Name);
            Dictionary<int, int> cart = Session[Resources.GlobalString.SESSION_CART] as Dictionary<int, int>;
            
            if (cart != null)
            {
                string PaymentSRu=null;
                string PaymentSEn = null;

                //Установка выбраной системы оплаты 
                //Setting the selected payment system
                if (PaymentSys == "1")
                {
                    PaymentSRu = Resources.ProductAndCRUD.InternalPaymentSystemRu;
                    PaymentSEn = Resources.ProductAndCRUD.InternalPaymentSystemEn;
                }
                else {
                    PaymentSRu = Resources.ProductAndCRUD.CreditCardRu;
                    PaymentSEn = Resources.ProductAndCRUD.CreditCardEn;
                }

                int iOrderStatusesNumber = orderStatusRepository.List().Count();
                string orderStatusRu = orderStatusRepository.List().First().NameRu;
                string orderStatusEn = orderStatusRepository.List().First().NameEn;
                Order or = null;

                if (!User.Identity.IsAuthenticated)
                {
                    // Заполнение заказа незарегистрированым пользователем
                    //Filling order unregistered users
                    or = of.OrderFillingUnknown(LastName, FirstName, Phone, Address, Email, PaymentSRu, PaymentSEn, 
                        iOrderStatusesNumber, orderStatusRu, orderStatusEn);
                }
                else
                {
                    //Заполнение заказа зарегистрированым пользователем
                    //Filling order registered users
                    or = of.OrderFillingProfile(User.Identity.Name, PaymentSRu, PaymentSEn, iOrderStatusesNumber, orderStatusRu, orderStatusEn);
                }
                
                orderRepository.Insert(or);
                orderRepository.Save();
                OrderID = or.ID;

                //перечисление добавленных товаров
                //enumeration added products
                foreach (int key in cart.Keys)
                {
                    var product = productRepository.Find(key); 

                    if (User.Identity.IsAuthenticated)
                    {
                        product.Price = User.IsInRole(E_shop.Constants.ROLE_USER) ? product.Price * 0.95m : product.Price * 0.9m;
                    }

                    //
                    product.CurrentBalance--;
                    productRepository.Save();

                    UserProfile profileAdmin = new UserProfile(Resources.GlobalString.ADMIN_NAME);
                    var profit = 0.0m;

                    if (PaymentSys.Equals("1"))
                    {
                        if (profile.Money > (decimal)product.Price * cart[key])
                        {
                            profile.Money = profile.Money - (decimal)product.Price * cart[key];
                            profile.Save();

                            profileAdmin.Money = profileAdmin.Money + (0.5m * (decimal)(product.Price - product.PurchasePrice) * cart[key]);

                            //Записать в отчет прибыль с заказа
                            //written to the database profit
                            profit = (0.5m * (decimal)(product.Price - product.PurchasePrice) * cart[key]);
                        }
                        else return RedirectToAction("Index");
                    }
                    else
                    {
                        profileAdmin.Money = profileAdmin.Money + (0.1m * (decimal)(product.Price - product.PurchasePrice));
                    }

                    // Заполение элементов заказа OrderItem
                    //Filling in the order about the product             
                    var orit = oif.OrderItemFillingMore(OrderID, key, product.NameRu, product.NameEn, cart[key], product.Price, profit); //, profit
                    orderItemRepository.Insert(orit);
                    orderItemRepository.Save();

                    profileAdmin.Save();

                    //Если была совершена покупка более чем установленная цена то он получает статус VIP
                    //If they were purchased more than the fixed price he will receive VIP status
                    if (User.Identity.IsAuthenticated && orderItem.Number * orderItem.Price > 30900)
                    {
                        if (Roles.IsUserInRole(User.Identity.Name, Constants.ROLE_USER))
                            Roles.RemoveUserFromRole(User.Identity.Name, Constants.ROLE_USER);
                        if (!Roles.IsUserInRole(User.Identity.Name, Constants.ROLE_VIPUSER))
                            Roles.AddUserToRole(User.Identity.Name, Constants.ROLE_VIPUSER);
                    }
                }

                Session.Remove(Resources.GlobalString.SESSION_CART);
            }

            return View("ThankYou");
        }
    }
}
