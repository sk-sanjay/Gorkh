using System;
using System.Collections.Generic;

namespace Application.ViewModels
{
    public class ProductsPurchasesCommonVM
    {
        //Products
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SubSubCatId { get; set; }
        public decimal ReservePrice { get; set; }
        public DateTime CreatedDate { get; set; }

        //for join
        public CategoriesVM Category { get; set; }
        public SubCategoriesVM SubCategory { get; set; }
        public SubSubCategoriesVM SubSubCategory { get; set; }

        //Buyer Fields
        public string BuyerFullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        //ProductsPurchases
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
    }
}
