using System;

namespace Application.ViewModels
{
    public class BuyerRequirementsVM
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? SubSubCategoryId { get; set; }
        public string Files { get; set; }
        public string Descrition { get; set; }
        public string FullName { get; set; }
        public string EmailID { get; set; }
        public string ProductListingTitle { get; set; }
        public int ProductCondition { get; set; }
        public int Qty { get; set; }
        public string SubSubCategoryName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public virtual CategoriesVM Category { get; set; }
        //public virtual SubCategoriesVM SubCategory { get; set; }
        //public virtual SubSubCategoriesVM SubSubCategory { get; set; }
    }
}
