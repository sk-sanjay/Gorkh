using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class ProductsBySeller
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
        public decimal ReservePrice { get; set; }
        public int SellerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsApprove { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual List<ProductsSpecifications> ProductsSpecifications { get; set; }

        //for join
        public virtual Categories Category { get; set; }
        public virtual SubCategories SubCategory { get; set; }

        [ForeignKey("SubSubCatId")]
        public virtual SubSubCategories SubSubCategory { get; set; }

        //public virtual ProductsLocations ProductsLocation { get; set; }
        //[ForeignKey("StateId")]
        //public virtual States State { get; set; }

        //products visitors
        public int? UserId { get; set; }
        public string UserType { get; set; }
        public int? totalVisitor { get; set; }

        //Products Interest
        public int BuyerId { get; set; }
        public int? TotalInterest { get; set; }
    }
}
