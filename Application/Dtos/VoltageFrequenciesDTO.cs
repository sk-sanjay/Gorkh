using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class VoltageFrequenciesDTO : BaseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Voltage Frequencies is required")]
        [StringLength(50, ErrorMessage = "{1} characters max")]
        public string VoltFrequency { get; set; }
    }
}
