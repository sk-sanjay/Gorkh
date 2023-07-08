namespace Application.ViewModels
{
    public class SpecificationsSSCategoriesVM : BaseVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int SubSubCatId { get; set; }
        public int SpecfId { get; set; }
        public int Sequence { get; set; }
        public bool IsMandatory { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string SubSubCategoriesName { get; set; }
        public string SpecfName { get; set; }
        public string TextType { get; set; }
        public string TextValue { get; set; }
        public int? Productchildid { get; set; }

        //public virtual CategoriesVM Category { get; set; }
        //public virtual SubCategoriesVM SubCategory { get; set; }
        //public virtual SubSubCategoriesVM SubSubCategory { get; set; }

    }
}
