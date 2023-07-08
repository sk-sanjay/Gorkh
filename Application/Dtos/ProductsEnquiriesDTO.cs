using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ProductsEnquiriesDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Descriptions { get; set; }
        public string EnquiryFile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
