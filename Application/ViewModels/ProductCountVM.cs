using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ProductCountVM
    {
        public int Id { get; set; }
        public int Approve { get; set; }
        public int Pending { get; set; }
    }
    public class ApprovePendingVM
    {
        public List<int> ApprovedCount { get; set; }
        public List<int> PendingCount { get; set; }

    }
}
