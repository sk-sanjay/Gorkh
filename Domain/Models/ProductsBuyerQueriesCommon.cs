using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class ProductsBuyerQueriesCommon
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SubSubCatId { get; set; }
        public decimal ReservePrice { get; set; }

        //for join
        public virtual Categories Category { get; set; }
        public virtual SubCategories SubCategory { get; set; }

        [ForeignKey("SubSubCatId")]
        public virtual SubSubCategories SubSubCategory { get; set; }

        //Products Buyer Queries
        public int ProductId { get; set; }
        public string Descriptions { get; set; }
        public string EnquiryFile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? BuyerId { get; set; }

        //Buyer Fields
        public string BuyerFullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
