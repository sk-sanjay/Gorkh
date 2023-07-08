using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
   public class BuyerSellerRegistrations1VM
    {
        public string Id { get; set; }
       // public string uid { get; set; }
        public string UniqueCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string LandlineNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int? SellerId { get; set; }
        public int? BuyerId { get; set; }
    }
}
