using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class SpecificationsSSCategories : BaseModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SubSubCatId { get; set; }
        public int SpecfId { get; set; }
        public int Sequence { get; set; }
        public bool IsMandatory { get; set; }

        [NotMapped]
        public string SpecfName { get; set; }
        [NotMapped]
        public string TextType { get; set; }
        public string TextValue { get; set; }
        public int? Productchildid { get; set; }
        //public virtual Categories Category{get; set;}
        //public virtual SubCategories SubCategory { get; set; }
        //public virtual SubSubCategories SubSubCategory { get; set; }
        //public virtual Specifications Specification { get; set; }
    }
}
