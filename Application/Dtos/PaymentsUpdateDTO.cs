using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class PaymentsUpdateDTO
    {
        public int Id { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? RecDate { get; set; }
    }
}
