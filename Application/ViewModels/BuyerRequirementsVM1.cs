using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class BuyerRequirementsVM1
    {
        public int Id { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string SubSubCategory { get; set; }
        public string Files { get; set; }
        public string Descrition { get; set; }

        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string ProductListingTitle { get; set; }
        public string ProductCondition { get; set; }
        public int Qty { get; set; }
        public string SubSubCategoryName { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateStr { get { return CreatedDate.HasValue ? CreatedDate.Value.ToShortDateString() : ""; } }
        public DateTime? ModifiedDate { get; set; }
    }
}
