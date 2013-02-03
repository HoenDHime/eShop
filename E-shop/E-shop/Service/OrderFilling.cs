using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace E_shop.Service
{
    public class OrderFilling
    {
        Order order = new Order();
        public Order OrderFillingProfile(string UserName, string paymentSystemRu, string paymentSystemEn, int iOrderStatusesNumber,
            string orderStatusRu, string orderStatusEn)
        {
            UserProfile profile = new UserProfile(UserName);
            order.FirstName = profile.FirstName;
            order.LastName = profile.LastName;
            order.Phone = profile.Phone;
            order.Address = profile.Address;
            order.UserName = UserName;
            order.Email = Membership.GetUser(UserName).Email;

            order.Date = DateTime.Now;
            order.PaymentSysRu = paymentSystemRu;
            order.PaymentSysEn = paymentSystemEn;
            order.OrderStatusRu = string.Empty;

            if (iOrderStatusesNumber > 0) {
                order.OrderStatusRu = orderStatusRu;
                order.OrderStatusEn = orderStatusEn;
            }

            return order;
        }

        public Order OrderFillingUnknown(string LastName, string FirstName, string Phone, string Address, string Email,
            string PaymentSysRu, string PaymentSysEn, int iOrderStatusesNumber, string orderStatusRu, string orderStatusEn)
        {
            order.Date = DateTime.Now;
            order.FirstName = FirstName;
            order.LastName = LastName;
            order.Phone = Phone;
            order.Address = Address;
            order.Email = Email;

            order.Date = DateTime.Now;
            order.PaymentSysRu = PaymentSysRu;
            order.PaymentSysEn = PaymentSysEn;
            order.OrderStatusRu = string.Empty;

            if (iOrderStatusesNumber > 0) 
            { 
                order.OrderStatusRu = orderStatusRu;
                order.OrderStatusEn = orderStatusEn; 
            }

            return order;

        }
    }
}