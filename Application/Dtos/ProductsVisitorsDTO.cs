using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ProductsVisitorsDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? UserId { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
