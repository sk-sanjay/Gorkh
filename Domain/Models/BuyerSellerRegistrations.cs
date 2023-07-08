using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class BuyerSellerRegistrations
    {
        public int Id { get; set; }
        public string UniqueCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string LandlineNo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyWebsite { get; set; }
        public int Organisation { get; set; }
        public int? OrganisationType { get; set; }
        public string Department { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string ZipCode { get; set; }
        public string Descriptionofproduct { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        [ForeignKey("Organisation")]
        public virtual Categories Category { get; set; }

        [ForeignKey("Country")]
        public virtual Countries Countries { get; set; }

        [ForeignKey("State")]
        public virtual States States { get; set; }

        [ForeignKey("City")]
        public virtual Cities Cities { get; set; }

        [ForeignKey("OrganisationType")]

        public virtual OrganisationTypes OrganisationTypes { get; set; }

        [NotMapped]
        public int SellerId { get; set; }
        
        [NotMapped]
        public int BuyerId { get; set; }

    }
}
