using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
  public  class VisitorRegistrationsDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Contact No. is required.")]
        public string ContactNo { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [DisplayName("Verify Email")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, ErrorMessage = "{1} characters max")]
        [Compare("Email", ErrorMessage = "The Email and Verify Email not match.")]
        public string VeryfyEmail { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; }
        public string Password { get; set; }
    }
}
