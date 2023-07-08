using System;

namespace Application.ViewModels
{
    public class ProductsBuyerQueriesVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Descriptions { get; set; }
        public string EnquiryFile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? BuyerId { get; set; }
    }
}
