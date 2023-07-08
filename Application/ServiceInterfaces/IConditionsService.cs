using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IConditionsService
    {
        //Common Methods

        Task<List<DropdownVM>> GetDropdown();
    }
}
