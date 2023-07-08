namespace Domain.Models
{
    public class SubSubCategories : BaseModel
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubSubCategoriesName { get; set; }
        public virtual Categories Category { get; set; }
        public virtual SubCategories SubCategory { get; set; }

    }
}
