using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class SubCategoriesDTO : BaseDTO
    {


        public int Id { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }
        [DisplayName("Sub-Category Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string SubCategoryName { get; set; }

        //[DisplayName("Short Name")]
        //[Required(ErrorMessage = "{0} is required")]
        //  public string ShortName { get; set; }



    }
}
