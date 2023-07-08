using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ITestimonialsService
    {
        Task<List<TestimonialsVM>> Get();
        Task<TestimonialsVM> Get(int id);
        Task<bool> CheckDuplicate(TestimonialsDTO argModelDto);
        Task<TestimonialsDTO> Create(TestimonialsDTO entity);
        Task<TestimonialsDTO> Update(TestimonialsDTO entity);
        Task<int> Delete(int id);

        Task<List<TestimonialsDTO>> CreateRange(List<TestimonialsDTO> entities);
        Task<List<TestimonialsDTO>> Upsert(List<TestimonialsDTO> entities);
        Task<int> DeleteRange(List<TestimonialsDTO> entities);

        //Custom Method
        Task<List<TestimonialsVM>> GetTestimonials();
    }
}
