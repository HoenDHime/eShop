using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_shop.Models.Repository
{
    public interface ISubcategoryProductRepository : IDisposable
    {
        SubcategoryProduct Find(int id);
        void Insert(SubcategoryProduct subcategory);      
        void Delete(int id);
        void Save();
        IEnumerable<SubcategoryProduct> List();
    }

    public class SubcategoryProductRepository : ISubcategoryProductRepository
    {
        Entities entities;

        public SubcategoryProductRepository(Entities entities)
        {
            this.entities = entities;
        }

        public IEnumerable<SubcategoryProduct> List()
        {
            var objs = entities.SubcategoryProducts;
            return objs;
        }

        public SubcategoryProduct Find(int id)
        {
            var obj = (from item in entities.SubcategoryProducts
                                 where item.ID == id
                                 select item).FirstOrDefault();
            return obj;
        }

        public void Insert(SubcategoryProduct subcategoryProduct)
        {
            entities.SubcategoryProducts.Add(subcategoryProduct);
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            entities.SubcategoryProducts.Remove(obj);
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