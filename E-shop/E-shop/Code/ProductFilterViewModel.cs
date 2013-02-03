using E_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_shop.Code
{
    public class ProductFilterViewModel
    {
        private readonly ISubcategoryRepository subcategoryRepository;
        private readonly IProductRepository productRepository;

        public ProductFilterViewModel(ISubcategoryRepository subcategoryRepository, IProductRepository productRepository) 
        {
            this.subcategoryRepository = subcategoryRepository;
            this.productRepository = productRepository;
        }

        public string ProductSearchName { get; set; }
        public List<SelectListItem> SubcategoryList { get; set; }
        public int SelectedSubcategoryID { get; set; }

        public void FillRu()
        {
            var subcategory = subcategoryRepository.List();

            SubcategoryList = subcategory.Select(a => new { a.NameRu, a.ID }).ToList().Select(a =>
                                new SelectListItem
                                {
                                    Text = a.NameRu,
                                    Value = a.ID.ToString(),
                                    Selected = a.ID == SelectedSubcategoryID
                                }).ToList();
        }

        public void FillEn()
        {
            var subcategory = subcategoryRepository.List();

            SubcategoryList = subcategory.Select(a => new { a.NameEn, a.ID }).ToList().Select(a =>
                                new SelectListItem
                                {
                                    Text = a.NameEn,
                                    Value = a.ID.ToString(),
                                    Selected = a.ID == SelectedSubcategoryID
                                }).ToList();
        }
    }
}