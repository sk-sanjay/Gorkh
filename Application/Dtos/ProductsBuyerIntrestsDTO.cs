using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ProductsBuyerIntrestsDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
