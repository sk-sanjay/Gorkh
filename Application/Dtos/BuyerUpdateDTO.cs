using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
   public class BuyerUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, ErrorMessage = "{1} characters max")]

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Enter a valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Primary Phone is required.")]
        [StringLength(10, ErrorMessage = "{1} characters max")]
        [RegularExpression("^[(0|91)?[7-9][0-9]{9}", ErrorMessage = "Enter a valid Mobile No.")]

        public string Mobile { get; set; }
        public string LandlineNo { get; set; }
       
        [DisplayName("Organisation")]
        [Required(ErrorMessage = "{0} is required")]
        public int Organisation { get; set; }
        public int OrganisationType { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, ErrorMessage = "{1} characters max")]
        public string Department { get; set; }
        [Required(ErrorMessage = "Please select Country.")]
        public int Country { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "{0} is required")]
        public int State { get; set; }


        [DisplayName("City")]
        [Required(ErrorMessage = "{0} is required")]
        public int City { get; set; }
        [Required(ErrorMessage = "Address Line 1 is required.")]
        [StringLength(200, ErrorMessage = "{1} characters max")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Address Line 2 is required.")]
        [StringLength(200, ErrorMessage = "{1} characters max")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "ZipCode is required.")]
        [StringLength(50, ErrorMessage = "{1} characters max")]
        public string ZipCode { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
      
       
    }
}
