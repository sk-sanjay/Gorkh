using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class SellerRegistrationsDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(300, ErrorMessage = "{1} characters max")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(300, ErrorMessage = "{1} characters max")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(300, ErrorMessage = "{1} characters max")]
        public string CompanyName { get; set; }
        public string CompanyWebsite { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [StringLength(300, ErrorMessage = "{1} characters max")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        //[Compare("Email", ErrorMessage = "{0} is not correct")]
        [DisplayName("Verify Email")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(300, ErrorMessage = "{1} characters max")]
        [Compare("Email", ErrorMessage = "The Email and Verify Email not match.")]
        public string VeryfyEmail { get; set; }

        [Required(ErrorMessage = "Primary Phone is required.")]
        [StringLength(300, ErrorMessage = "{1} characters max")]
        public string Mobile { get; set; }
        public string LandlineNo { get; set; }

        [Required(ErrorMessage = "Please select Organisation.")]
        public int Organisation { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(300, ErrorMessage = "{1} characters max")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Please select Country.")]
        public int Country { get; set; }
        [Required(ErrorMessage = "Please select State")]
        public int State { get; set; }


        [Required(ErrorMessage = "Please select City.")]
        public int City { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required.")]
        [StringLength(500, ErrorMessage = "{1} characters max")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Address Line 2 is required.")]
        [StringLength(500, ErrorMessage = "{1} characters max")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "ZipCode is required.")]
        [StringLength(100, ErrorMessage = "{1} characters max")]
        public string ZipCode { get; set; }
        public string Descriptionofproduct { get; set; }
        public bool TermsAndConditions { get; set; }

        public string Password { get; set; }

        //[Range(typeof(bool), "true", "true", ErrorMessage = "You gotta tick the box!")]
        //public bool TermsAndConditions { get; set; }

        //[Display(Name = "I accept the above terms and conditions.")]
        //[CheckBoxValidation(ErrorMessage = "Please accept the terms and condition.")]

        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
