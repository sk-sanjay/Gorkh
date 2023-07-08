using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class SpecificationsSSCategoriesDTO : BaseDTO
    {
        public int Id { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }

        [DisplayName("Sub Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int SubCategoryId { get; set; }

        [DisplayName("Sub Sub Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int SubSubCatId { get; set; }

        [DisplayName("Specification")]
        [Required(ErrorMessage = "{0} is required")]
        public int SpecfId { get; set; }

        [DisplayName("Sequence No")]
        [Required(ErrorMessage = "{0} is required")]
        public int Sequence { get; set; }
        public bool IsMandatory { get; set; }
    }
}
