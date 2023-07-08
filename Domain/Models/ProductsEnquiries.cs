using System;
namespace Domain.Models
{
    public class ProductsEnquiries
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Descriptions { get; set; }
        public string EnquiryFile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
