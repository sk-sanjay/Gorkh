namespace Application.ViewModels
{
    public class SubCategoriesVM : BaseVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public virtual CategoriesVM Category { get; set; }
        //public string ShortName { get; set; }

        //public int Id { get; set; }
        //public int CategoryId { get; set; }
        //public string EnglishName { get; set; }
        //public string HindiName { get; set; }
        //public string ShortName { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public bool Status { get; set; }
    }
}
