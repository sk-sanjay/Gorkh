using Application.ServiceInterfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.Services
{
    public class ConditionsService : IConditionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ConditionsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Custom Methods
        public async Task<List<DropdownVM>> GetDropdown()
        {
            var models = await _unitOfWork.ConditionsRepo.GetActive().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<DropdownVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            ////Move India to first position
            //var index = modelVms.FindIndex(x => x.Text == "India");
            //var item = modelVms[index];
            //modelVms[index] = modelVms[0];
            //modelVms[0] = item;
            return modelVms;
        }
    }
}
