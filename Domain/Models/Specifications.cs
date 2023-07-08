namespace Domain.Models
{
    public class Specifications : BaseModel
    {
        public int Id { get; set; }
        public string SpecfName { get; set; }
        public string TextType { get; set; }
        public bool IsCommon { get; set; }
    }
}
