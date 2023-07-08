using System;
using System.Collections.Generic;
namespace Application.ViewModels
{
    public class ProductsVM //: BaseVM
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public string SaleType { get; set; }
        public string SellerType { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SubSubCatId { get; set; }

        public int ConditionId { get; set; }
        public int QuantityAvl { get; set; }
        public string YearofProc { get; set; }
        public string InventoryId { get; set; }
        public string SerialNo { get; set; }
        public decimal EstimatePrice { get; set; }
        public decimal ReservePrice { get; set; }
        public int SellerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsApprove { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int? FinalSubmit { get; set; }
        public List<ProductsSpecificationsVM> ProductsSpecifications { get; set; }

        //for join
        public CategoriesVM Category { get; set; }
        public SubCategoriesVM SubCategory { get; set; }
        public SubSubCategoriesVM SubSubCategory { get; set; }
        //public ProductsLocationsVM ProductsLocation { get; set; }
        //public StatesVM State { get; set; }
    }
}
