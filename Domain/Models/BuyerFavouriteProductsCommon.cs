using System;
namespace Domain.Models
{
  public  class BuyerFavouriteProductsCommon
    {
        //Products Interest
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public DateTime CreatedDate { get; set; }

        //products
        public string ProductNumber { get; set; }
        public string SaleType { get; set; }
        public string SellerType { get; set; }
        public decimal ReservePrice { get; set; }

        //SubSubCategories
        public string SubSubCategoriesName { get; set; }
        //Buyer Fields
        public string BuyerFullName { get; set; }
    }
}
