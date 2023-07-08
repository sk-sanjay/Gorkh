using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
   public class BuyerOffersCommon
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public string ProductNumber { get; set; }
        public decimal EstimatePrice { get; set; }
        public decimal OfferdPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmailId { get; set; }
        //public bool IsSoled { get; set; }
        public string IsSoled { get; set; }
    }
}
