using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CategoriesDTO : BaseDTO
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Short Name is required")]
        [StringLength(3, ErrorMessage = "{1} characters max")]
        public string ShortName { get; set; }



        //[Key]
        //public int Id { get; set; }
        //[Required(ErrorMessage = "Please Enter Category Name")]
        //[StringLength(100)]
        //public string EnglishName { get; set; }
        //public string HindiName { get; set; }

        //[Required(ErrorMessage = "Please Enter Remarks")]
        //[StringLength(100)]
        //public string Remarks { get; set; }
        //public bool Status { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }

    }
}
