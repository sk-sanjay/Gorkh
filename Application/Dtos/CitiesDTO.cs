using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CitiesDTO : BaseDTO
    {
        public int Id { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "{0} is required")]
        public int CountryId { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "{0} is required")]
        public int StateId { get; set; }

        [DisplayName("City Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        public string CityName { get; set; }

    }
}
