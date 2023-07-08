
namespace Domain.Models
{
    public class Cities : BaseModel
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
        public virtual Countries Country { get; set; }
        //public virtual States State { get; set; }
    }
}
