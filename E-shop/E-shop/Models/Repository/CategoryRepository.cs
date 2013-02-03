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
    public interface ICategoryRepository : IDisposable
    {
        Category Find(int id);
        void Insert(Category category);
        //void Update(int id, FormCollection collection);
        void Delete(int id);
        void Save();
        IEnumerable<Category> List();
    }

    public class CategoryRepository : ICategoryRepository
    {
        Entities entities;

        public CategoryRepository(Entities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<Category> List()
        {
            var objs = entities.Categories;
            return objs;
        }

        public Category Find(int id)
        {
            var obj = (from item in entities.Categories
                       where item.ID == id
                       select item).FirstOrDefault();
            return obj;
        }

        public void Insert(Category category)
        {
            entities.Categories.Add(category);
        }


        //public void Update(int id, FormCollection collection)
        //{
        //    var obj = Find(id);
        //    entities.Categories.Add(obj);
        //}

        public void Delete(int id)
        {
            var obj = Find(id);
            entities.Categories.Remove(obj);
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