using System.Collections.Generic;
namespace Domain.Models
{
    public class Categories : BaseModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        //For geting child data
        public virtual List<SubCategories> SubCategory { get; set; }

    }
}
