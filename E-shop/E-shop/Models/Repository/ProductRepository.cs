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
    public interface IProductRepository : IDisposable
    {
        Product Find(int id);
        void Insert(Product product);
        //void Update(int id, FormCollection collection);
        void Delete(int id);
        void Save();
        IEnumerable<Product> List();
    }

    public class ProductRepository : IProductRepository
    {
        Entities entities;

        public ProductRepository(Entities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<Product> List()
        {
            var objs = entities.Products;
            return objs;
        }

        public Product Find(int id)
        {
            var obj = (from item in entities.Products
                               where item.ID == id
                               select item).FirstOrDefault();
            return obj;
        }

        public void Insert(Product product)
        {
            entities.Products.Add(product);
        }


        //public void Update(int id, FormCollection collection)
        //{
        //    var obj = Find(id);
        //    entities.Products.Add(obj);
        //}

        public void Delete(int id)
        {
            var obj = Find(id);
            entities.Products.Remove(obj);
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