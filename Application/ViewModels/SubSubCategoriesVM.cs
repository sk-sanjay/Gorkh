namespace Application.ViewModels
{
    public class SubSubCategoriesVM : BaseVM
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubSubCategoriesName { get; set; }
        public virtual CategoriesVM Category { get; set; }
        public virtual SubCategoriesVM SubCategory { get; set; }

    }
}
