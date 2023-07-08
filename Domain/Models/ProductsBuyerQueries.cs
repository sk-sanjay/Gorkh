﻿using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class ProductsBuyerQueries
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Descriptions { get; set; }
        public string EnquiryFile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? BuyerId { get; set; }

        public IEnumerable<object> Select(Func<object, object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
