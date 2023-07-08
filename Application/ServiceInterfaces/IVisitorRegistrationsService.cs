using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
   public interface IVisitorRegistrationsService
    {
        Task<List<VisitorRegistrationsVM>> Get();
        Task<VisitorRegistrationsVM> Get(int id);
        Task<VisitorRegistrationsDTO> Create(VisitorRegistrationsDTO entity);
        Task<VisitorRegistrationsDTO> Update(VisitorRegistrationsDTO entity);
        Task<int> Delete(int id);

        Task<List<VisitorRegistrationsDTO>> CreateRange(List<VisitorRegistrationsDTO> entities);
        Task<List<VisitorRegistrationsDTO>> Upsert(List<VisitorRegistrationsDTO> entities);
        Task<int> DeleteRange(List<VisitorRegistrationsDTO> entities);
        Task<bool> CheckDuplicate(VisitorRegistrationsDTO argModelDto);
        Task<VisitorRegistrationsVM> GetbyEmailId(string email);
    }
}
