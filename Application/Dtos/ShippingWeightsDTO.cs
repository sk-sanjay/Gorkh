using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ShippingWeightsDTO : BaseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Unit name is required")]
        [StringLength(50, ErrorMessage = "{1} characters max")]
        public string UnitName { get; set; }

        [StringLength(2, ErrorMessage = "{1} characters max")]
        public string UnitCode { get; set; }
    }
}
