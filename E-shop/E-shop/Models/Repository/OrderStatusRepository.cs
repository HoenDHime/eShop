using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_shop.Models.Repository
{
    public interface IOrderStatusRepository : IDisposable
    {
        OrderStatus Find(int id);
        void Insert(OrderStatus orderstatu);
        void Update(int id, FormCollection collection);
        void Delete(int id);
        void Save();
        IEnumerable<OrderStatus> List();
    }

    public class OrderStatusRepository : IOrderStatusRepository
    {
        Entities entities;

        public OrderStatusRepository(Entities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<OrderStatus> List()
        {
            var objs = entities.OrderStatus;
            return objs;
        }

        public OrderStatus Find(int id)
        {
            var obj = (from item in entities.OrderStatus
                       where item.ID == id
                       select item).FirstOrDefault();
            return obj;
        }

        public void Insert(OrderStatus orderstatu)
        {
            entities.OrderStatus.Add(orderstatu);
        }


        public void Update(int id, FormCollection collection)
        {
            var obj = Find(id);
            entities.OrderStatus.Add(obj);
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            entities.OrderStatus.Remove(obj);
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