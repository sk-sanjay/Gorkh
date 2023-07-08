using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ManufacturersDTO : BaseDTO
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "{0} is required")]
        [Required(ErrorMessage = "Manufacturer Name is required")]
        [StringLength(50, ErrorMessage = "{1} characters max")]
        public string MfrName { get; set; }
    }
}
