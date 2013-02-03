using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using E_shop.Models;
using E_shop.Controllers;
using E_shop;
using System.Linq;

namespace E_shopTests
{
    [TestClass]
    public class CategoryControllerTests
    {
        private IEnumerable<Category> cat;
        private IEnumerable<Subcategory> sub;
        Error err;
        private Category catOne;
        Mock<ICategoryRepository> category_mock = new Mock<ICategoryRepository>();
        Mock<ISubcategoryRepository> subcategorymock = new Mock<ISubcategoryRepository>();
        //private ICategoryRepository categoryRepository;
        //private ISubcategoryRepository subcategoryRepository;

        [TestInitialize]
        public void PreTestingInitialize() 
        {
            category_mock.Setup(x => x.List()).Returns(new Category[] { 
            new Category() { NameEn = "Green",ID=1 },
            new Category() { NameEn = "Blue", ID=2 }
              });

            Subcategory[] sub = new Subcategory[] { 
            new Subcategory() { NameEn = "LG",ID=1 },
            new Subcategory() { NameEn = "Nokia", ID=2 }
              };
        }

        //public interface IMyInterface 
        //{
        //    string PregressMessage(string message);
        //}

        [TestMethod]
        public void CategoryIndex()
        {
           //category_mock.Setup(m => m.List()).Returns(cat);
        }

        [TestMethod]
        public void CategoryEdit()
        {
            //mock.Setup(m => m.Find(2)).Returns(catOne);

            //foreach (Category c in cat)
            //{
            //    mock.Verify(m => m.Save(), Times.Once());
            //}

            //mock.Setup(m => m.Save());
        }

        [TestMethod]
        public void CategoryDetails()
        {
            CategoryController catcont = new CategoryController(category_mock.Object, subcategorymock.Object, err);

            //var result = catcont.Details(1);

            //var category = result.ViewData.Model as List<Category>;

            //var find=category_mock.Object.Find(1);

            var result = catcont.Details(1);

            //Assert.AreEqual(find, result); //category[0].NameEn
        }

        //private class FakeCategoryRepository : ICategoryRepository
        //{
        //    private Category[] cat = { new Category() { NameEn = "Green", NameRu = "Зеленый" } };

        //    public IEnumerable<Category> List() { return cat; }

        //    public void Insert(Category category) {  }

        //    public void Save() {  }

        //    public Category Find(int id) { return cat.Where(x=>x.ID==id).FirstOrDefault(); }

        //    public void Delete(int id) { }

        //    public void Dispose() { }
        //}
    }
}
