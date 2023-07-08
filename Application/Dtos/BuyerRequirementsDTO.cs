using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Application.Dtos
{
    public class BuyerRequirementsDTO
    {
        public int Id { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }
        [DisplayName(" Sub-Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int SubCategoryId { get; set; }
        //[DisplayName(" Sub-Sub-Category")]
        //[Required(ErrorMessage = "{0} is required")]
        public int? SubSubCategoryId { get; set; }
        public string Files { get; set; }
        public string Descrition { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessage = " Email ID is required")]
        public string EmailID { get; set; }
        [Required(ErrorMessage = " Product Listing Title is required")]
        public string ProductListingTitle { get; set; }
        [DisplayName("Product Condition")]
        [Required(ErrorMessage = "{0} is required")]
        public int ProductCondition { get; set; }
        [DisplayName("Qty")]
        [Required(ErrorMessage = "{0} is required")]
        public int Qty { get; set; }
        [DisplayName("Sub-Sub-Category Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string SubSubCategoryName { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public IFormFile upload1 { get; set; }

    }
}
