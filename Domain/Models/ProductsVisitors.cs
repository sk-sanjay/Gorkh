using System;
namespace Domain.Models
{
    public class ProductsVisitors
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? UserId { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
