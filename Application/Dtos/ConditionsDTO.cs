using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class ConditionsDTO : BaseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Condition Name is required")]
        [StringLength(50, ErrorMessage = "{1} characters max")]
        public string ConditionName { get; set; }
    }
}
