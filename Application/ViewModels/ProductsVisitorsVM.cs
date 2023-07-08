using System;

namespace Application.ViewModels
{
    public class ProductsVisitorsVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? UserId { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
