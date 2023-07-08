using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class BuyerOffersInsertDTO
    {
        [Required(ErrorMessage = "Offered required")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OfferdPrice { get; set; }
        
    }
}
