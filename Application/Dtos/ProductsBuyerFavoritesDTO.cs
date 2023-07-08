using System;

namespace Application.Dtos
{
    public class ProductsBuyerFavoritesDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
