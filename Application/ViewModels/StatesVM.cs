using System.Collections.Generic;

namespace Application.ViewModels
{
    public class StatesVM : BaseVM
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string CountryName { get; set; }
        public CountriesVM Country { get; set; }
        public List<CitiesVM> Cities { get; set; }
    }
}
