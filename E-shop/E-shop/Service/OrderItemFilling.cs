using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_shop.Service
{
    public class OrderItemFilling
    {
        OrderItem orderItem = new OrderItem();
        public OrderItem OrderItemFillingMore(int ID, int key, string NameRu, string NameEn, int cartKey, decimal? price, decimal? profit) //, decimal? profit
        {
            orderItem.OrderID = ID;
            orderItem.ProductID = key;
            orderItem.NameRu = NameRu;
            orderItem.NameEn = NameEn;
            orderItem.Number = cartKey;
            orderItem.Price = price;
            orderItem.Profit = profit;

            return orderItem;
        }
    }
}