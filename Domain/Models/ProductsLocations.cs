namespace Domain.Models
{
    public class ProductsLocations
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
    }
}
