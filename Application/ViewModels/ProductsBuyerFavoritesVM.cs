using System;

namespace Application.ViewModels
{
   public class ProductsBuyerFavoritesVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
