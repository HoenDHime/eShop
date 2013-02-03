using E_shop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace E_shop.Models
{
    public interface IOrderItemRepository : IDisposable
    {
        OrderItem Find(int id);
        void Insert(OrderItem orderitem);
        void Update(int id, FormCollection collection);
        void Delete(int id);
        void Save();
        IEnumerable<OrderItem> List();
    }

    public class OrderItemRepository : IOrderItemRepository
    {
        Entities entities;

        public OrderItemRepository(Entities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<OrderItem> List()
        {
            var objs = entities.OrderItems;
            return objs;
        }

        public OrderItem Find(int id)
        {
            var obj = (from item in entities.OrderItems
                       where item.ID == id
                       select item).FirstOrDefault();
            return obj;
        }

        public void Insert(OrderItem orderotem)
        {
            entities.OrderItems.Add(orderotem);
        }


        public void Update(int id, FormCollection collection)
        {
            var obj = Find(id);
            entities.OrderItems.Add(obj);
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            entities.OrderItems.Remove(obj);
        }

        public void Save()
        {
            entities.SaveChanges();
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}