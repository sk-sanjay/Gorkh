using System.Collections.Generic;
namespace Application.ViewModels
{
    public class CategoriesVM : BaseVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        //public int Id { get; set; }
        //public string EnglishName { get; set; }
        //public string HindiName { get; set; }

        //public string Remarks { get; set; }
        //public bool Status { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }

        //For geting child data
        public List<SubCategoriesVM> SubCategory { get; set; }
    }
}
