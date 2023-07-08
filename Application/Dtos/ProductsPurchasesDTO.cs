using System;

namespace Application.Dtos
{
    public class ProductsPurchasesDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
