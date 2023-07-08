using System.ComponentModel.DataAnnotations;
namespace Application.Dtos
{
    public class SpecificationsDTO : BaseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field name is required")]
        [StringLength(50, ErrorMessage = "{1} characters max")]
        public string SpecfName { get; set; }
        public string TextType { get; set; }
        public bool IsCommon { get; set; }
    }
}
