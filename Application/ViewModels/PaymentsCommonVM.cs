using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Application.ViewModels
{
    public class PaymentsCommonVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public int PaymentModeId { get; set; }
        public string DdChequeNo { get; set; }
        public DateTime? ChAndBgDate { get; set; }
        public string DrawnOn { get; set; }
        public decimal AmountRp { get; set; }
        public string BgNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? RecDate { get; set; }
        public int? TotalPaidBuyer { get; set; }

        //Products
        public int SellerId { get; set; }
        public string ProductNumber { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SubSubCatId { get; set; }
        public decimal ReservePrice { get; set; }

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
    }
}
