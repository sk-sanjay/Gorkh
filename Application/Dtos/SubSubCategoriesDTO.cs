using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class SubSubCategoriesDTO : BaseDTO
    {
        public int Id { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }

        [DisplayName("Sub-Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int SubCategoryId { get; set; }

       

        [DisplayName("Sub-Sub-Categories Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        public string SubSubCategoriesName { get; set; }
    }
}
