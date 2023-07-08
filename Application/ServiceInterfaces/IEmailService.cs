using Application.ViewModels;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string ToAddress, string Subject, string Body, string CcAddress, string BccAddress, List<Attachment> Attachment1);
        Task SendEmailAsync(string ToAddress, string Subject, string Body, string CcAddress, string BccAddress, Attachment Attachment1, Attachment Attachment2);
        //Task SendEmailAsync(EmailVM EmailVm);
       // Task OnTaskCompleted(object sender, NotifierEventArgs args);
    }
}