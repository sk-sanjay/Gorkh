using System;
namespace Application.ViewModels
{
    public class ProductsEnquiriesVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Descriptions { get; set; }
        public string EnquiryFile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
