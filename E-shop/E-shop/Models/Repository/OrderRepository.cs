using E_shop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace E_shop.Models
{
    public interface IOrderRepository : IDisposable
    {
        Order Find(int id);
      //  void Update(int id, FormCollection collection);
        void Update(Order order);
        void Insert(Order order);
        void Delete(int id);
        void Save();
        IEnumerable<Order> List();
    }

    public class OrderRepository : IOrderRepository
    {
        Entities entities;

        public OrderRepository(Entities entities)
        {
            this.entities = entities;
        }
 
        public IEnumerable<Order> List()
        {
            var objs = entities.Orders;
            return objs;
        }

        public Order Find(int id)
        {
            var obj = (from item in entities.Orders
                       where item.ID == id
                       select item).FirstOrDefault();
            return obj;
        }

        public void Insert(Order order)
        {
            entities.Orders.Add(order);
        }

        public void Update(Order order)
        {
            entities.Orders.Add(order);
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            entities.Orders.Remove(obj);
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