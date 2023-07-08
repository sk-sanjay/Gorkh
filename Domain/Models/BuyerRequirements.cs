using System;

namespace Domain.Models
{
    public class BuyerRequirements
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? SubSubCategoryId { get; set; }
        public string Files { get; set; }
        public string Descrition { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string ProductListingTitle { get; set; }
        public int ProductCondition { get; set; }
        public int Qty { get; set; }
        public string SubSubCategoryName { get; set; }
    }
}
