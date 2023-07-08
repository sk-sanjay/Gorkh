using System;

namespace Domain.Models
{
    public class BuyerOffers
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public string ProductNumber { get; set; }
        public decimal EstimatePrice { get; set; }
        public decimal OfferdPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; }
        //public bool IsSoled { get; set; }
        public string IsSoled { get; set; }
    }
}
