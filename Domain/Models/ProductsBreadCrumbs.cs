using System;

namespace Domain.Models
{
    public class ProductsBreadCrumbs
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public int SubCatid { get; set; }
        public string SubCatName { get; set; }
    }
}
