using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ProductsLocationsDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please select Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Please select State")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "Please select City")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
    }
}
