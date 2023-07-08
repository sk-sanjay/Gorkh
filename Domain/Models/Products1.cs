using System;
namespace Domain.Models
{
    public class Products1
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public int CategoryId { get; set; }
        public int SubSubCatId { get; set; }
        public int SubCategoryId { get; set; }
        public int QuantityAvl { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal EstimatePrice { get; set; }
        public decimal ReservePrice { get; set; }
        public string SubSubCategoriesName { get; set; }
        public string ProductImage { get; set; }
        public string StateName { get; set; }
        public string CategoryName { get; set; }
        public string Descriptions { get; set; }
    }
}
