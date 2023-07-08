﻿using System.Collections.Generic;

namespace Application.Dtos.Datatables
{
    public class DatatableResponse<T> where T : class
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<T> data { get; set; }
    }
}
