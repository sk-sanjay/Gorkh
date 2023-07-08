using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
   public class OurCustomersDTO : BaseDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(100, ErrorMessage = "{1} characters max")]
        public string CompanyName { get; set; }
        public string Logo { get; set; }
    }
}
