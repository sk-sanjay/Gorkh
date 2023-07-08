namespace Application.ViewModels
{
    public class CitiesVM : BaseVM
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
        public virtual CountriesVM Country { get; set; }
        //public virtual StatesVM State { get; set; }
    }
}
