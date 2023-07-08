using System;

namespace Application.Dtos.Datatables
{
    public class DatatableRequest
    {
        public string draw { get; set; }
        public int? start { get; set; }
        public int? length { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDirection { get; set; }
        public string searchValue { get; set; }
        public int skip => start != null ? Convert.ToInt32(start) : 0;
        public int pageSize => length != null ? Convert.ToInt32(length) : 30;
    }
}
