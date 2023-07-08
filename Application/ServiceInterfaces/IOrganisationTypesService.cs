using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IOrganisationTypesService
    {
        Task<List<OrganisationTypesVM>> Get();
        //Task<OrganisationTypesVM> Get(int id);

        //Custom Methods
        Task<List<DropdownVM>> GetDropdown();
    }
}
