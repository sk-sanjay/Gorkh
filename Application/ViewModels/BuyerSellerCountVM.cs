using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
  public class BuyerSellerCountVM
    {
        public int Id { get; set; }
        public int BuyerCount { get; set; }
        public int SellerCount { get; set; }
        public int BothCount { get; set; }
    }
}
