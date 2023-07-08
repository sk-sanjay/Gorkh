using System;

namespace Domain.Models
{
   public class BuyerUpdate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string LandlineNo { get; set; }
        public int Organisation { get; set; }
        public int OrganisationType { get; set; }
        public string Department { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
