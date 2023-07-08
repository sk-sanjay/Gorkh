using System.Collections.Generic;

namespace Application.ViewModels
{
    public class DashboardStatsVM
    {
        public string CardHeader { get; set; }

        public List<DashboardTableRow> Rows { get; set; }
        //public int RegInstCount { get; set; }
        //public int CompProfileRegInstCount { get; set; }
        //public int AddedAsdcCount { get; set; }
        //public int AptisExamScheduleBatches { get; set; }
        //public int AptisExamNotScheduleBatches { get; set; }
        //public int AptisExamConductedBatches { get; set; }
        //public int AptisTotalBatches { get; set; }
    }

    public class DashboardTableRow
    {
        public string DisplayText { get; set; }
        public string IconClass { get; set; }
        public string TargetUrl { get; set; }
        public string TargetUrlText { get; set; }
        public string TargetUrlIconClass { get; set; }
        public string BoxBackgroundClass { get; set; }
        public int Count { get; set; }
        public int Count1 { get; set; }
    }
}
