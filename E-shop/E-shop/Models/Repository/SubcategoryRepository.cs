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
    public interface ISubcategoryRepository : IDisposable
    {
        Subcategory Find(int id);
        void Insert(Subcategory subcategory);
        //void Update(int id, FormCollection collection);
        void Delete(int id);
        void Save();
        IEnumerable<Subcategory> List();
    }

    public class SubcategoryRepository : ISubcategoryRepository
    {
        Entities entities;

        public SubcategoryRepository(Entities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<Subcategory> List()
        {
            var objs = entities.Subcategories;
            return objs;
        }

        public Subcategory Find(int id)
        {
            var obj = (from item in entities.Subcategories
                                 where item.ID == id
                                 select item).FirstOrDefault();
            return obj;
        }

        public void Insert(Subcategory subcategory)
        {
            entities.Subcategories.Add(subcategory);
        }


        //public void Update(int id, FormCollection collection)
        //{
        //    var obj = Find(id);
        //    entities.Subcategories.Add(obj);
        //}

        public void Delete(int id)
        {
            var obj = Find(id);
            entities.Subcategories.Remove(obj);
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