using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ProductsDTO //: BaseDTO
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public string SaleType { get; set; }
        public string SellerType { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }

        [DisplayName("Sub Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int SubCategoryId { get; set; }

        [DisplayName("Sub Sub Category")]
        [Required(ErrorMessage = "{0} is required")]
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
        //public List<ProductsDTO> ProductsSpecifications { get; set; }
        //[InverseProperty("Country")]
        public List<ProductsSpecificationsDTO> ProductsSpecifications { get; set; }
    }
}
