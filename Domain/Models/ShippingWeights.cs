namespace Domain.Models
{
    public class ShippingWeights : BaseModel
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
        public string UnitCode { get; set; }

    }
}
