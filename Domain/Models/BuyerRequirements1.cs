using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
  public  class BuyerRequirements1
    {
        public int Id { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string SubSubCategory { get; set; }
        public string Files { get; set; }
        public string Descrition { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string ProductListingTitle { get; set; }
        public string ProductCondition { get; set; }
        public int Qty { get; set; }
        public string SubSubCategoryName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
