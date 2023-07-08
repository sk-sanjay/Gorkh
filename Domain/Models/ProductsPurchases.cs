using System;

namespace Domain.Models
{
    public class ProductsPurchases
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
