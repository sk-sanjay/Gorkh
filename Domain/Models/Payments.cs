﻿using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Payments
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public int PaymentModeId { get; set; }
        public string DdChequeNo { get; set; }
        public DateTime? ChAndBgDate { get; set; }
        public string DrawnOn { get; set; }
        public decimal AmountRp { get; set; }
        public string BgNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? RecDate { get; set; }
    }
}
