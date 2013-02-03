using E_shop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace E_shop.Models
{
    public interface IPaymentSystemRepository : IDisposable
    {
        PaymentSystem Find(int id);
        void Insert(PaymentSystem paymentsystem);
        void Update(int id, FormCollection collection);
        void Delete(int id);
        void Save();
        IEnumerable<PaymentSystem> List();
    }

    public class PaymentSystemRepository : IPaymentSystemRepository//  IRepository<PaymentSystem> //
    {
        Entities entities;

        public PaymentSystemRepository(Entities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<PaymentSystem> List()
        {
            var objs = entities.PaymentSystems;
            return objs;
        }      

        public PaymentSystem Find(int id)
        {
            var obj = (from item in entities.PaymentSystems 
                                 where item.ID == id 
                                 select item).FirstOrDefault();
            return obj;
        }

        public void Insert(PaymentSystem paymentsystem)
        {
            entities.PaymentSystems.Add(paymentsystem);
        }


        public void Update(int id, FormCollection collection)
        {
            var obj = (from item in entities.PaymentSystems where item.ID == id select item).FirstOrDefault();
            entities.PaymentSystems.Add(obj);
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            entities.PaymentSystems.Remove(obj);
            entities.SaveChanges();

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