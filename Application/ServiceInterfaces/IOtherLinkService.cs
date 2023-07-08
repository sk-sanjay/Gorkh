using Application.Dtos;
using Application.ViewModels;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
   public interface IOtherLinkService
    {
        Task<List<OtherLinkDTO>> Get();
        Task<OtherLinkDTO> Add(OtherLinkDTO productDto);
        Task<OtherLinkDTO> Get(int id);
        Task<OtherLinkVM> Get(string Heading);
        Task<OtherLinkDTO> Update(OtherLinkDTO productDto);
        Task<List<OtherLinkVM>> GetCategoriesWithAll();
        //Task<int> Remove(int id);
        //Task<List<OtherLinkVM>> GetCategoriesWithAllTest(int id);
        //Task<OtherLinkDTO> EditMenuContent(OtherLinkDTO productDto);
      
       
    }
}
