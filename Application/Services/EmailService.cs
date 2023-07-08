using Application.AppSettings;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
//using System.Net.Mail;
using System.Threading.Tasks;
using Attachment = System.Net.Mail.Attachment;
namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        //public async Task SendEmailAsync(EmailVM EmailVm)
        //{
        //    await SendEmail(EmailVm).ConfigureAwait(false);
        //}

        //public async Task OnTaskCompleted(object sender, NotifierEventArgs args)
        //{
        //    await SendEmail(args.EmailVm).ConfigureAwait(false);
        //}
        public Task SendEmailAsync(string ToAddress, string Subject, string Body, string CcAddress, string BccAddress, Attachment Attachment1, Attachment Attachment2)
        {
            List<Attachment> lstAttachment = new List<Attachment>();
            if (Attachment1 != null)
            {
                lstAttachment.Add(Attachment1);
            }
            if (Attachment2 != null)
            {
                lstAttachment.Add(Attachment2);
            }
            SendEmail(ToAddress, Subject, Body, CcAddress, BccAddress, lstAttachment);
            return Task.CompletedTask;
        }
        public Task SendEmailAsync(string ToAddress, string Subject, string Body, string CcAddress, string BccAddress, List<Attachment> Attachment1)
        {
            SendEmail(ToAddress, Subject, Body, CcAddress, BccAddress, Attachment1);
            return Task.CompletedTask;
        }
        private void SendEmail(string ToAddress, string Subject, string Body, string CcAddress, string BccAddress, List<Attachment> Attachment1)
        {
            try
            {
               

                var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromAddress, _emailSettings.DisplayName),
                    Subject = Subject,
                    Body = Body
                };
                //message.To.Add(new MailAddress(ToAddress));
                string[] Multi = ToAddress.Split(';');
                foreach (string Multiemailid in Multi)
                {
                    message.To.Add(new MailAddress(Multiemailid));
                }
                if (!string.IsNullOrEmpty(CcAddress))
                    message.CC.Add(new MailAddress(CcAddress));
                if (!string.IsNullOrEmpty(BccAddress))
                    message.Bcc.Add(new MailAddress(BccAddress));
                message.IsBodyHtml = true;
                if (Attachment1 != null && Attachment1.Count > 0)
                {
                    foreach (var item in Attachment1)
                    {
                        message.Attachments.Add(item);
                    }
                }
                var smtpClient = new SmtpClient
                {
                    Host = _emailSettings.Host,
                    Port = _emailSettings.Port,
                    EnableSsl = false,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password)
                };
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
            }
        }

        //private async Task SendEmail(EmailVM EmailVm)
        //{
        //    //var message = new MailMessage
        //    //{
        //    //    From = new MailAddress(_emailSettings.FromAddress, _emailSettings.DisplayName),
        //    //    Subject = EmailVm.Subject,
        //    //    Body = EmailVm.Body,
        //    //    IsBodyHtml = EmailVm.IsBodyHtml
        //    //};
        //    //foreach (var ToAddress in EmailVm.ToAddresses)
        //    //    message.To.Add(new MailAddress(ToAddress));
        //    //if (EmailVm.CcAddresses != null && EmailVm.CcAddresses.Count > 0)
        //    //{
        //    //    foreach (var CcAddress in EmailVm.CcAddresses)
        //    //        message.CC.Add(new MailAddress(CcAddress));
        //    //}
        //    //if (EmailVm.BccAddresses != null && EmailVm.BccAddresses.Count > 0)
        //    //{
        //    //    foreach (var BccAddress in EmailVm.BccAddresses)
        //    //        message.Bcc.Add(new MailAddress(BccAddress));
        //    //}
        //    //if (EmailVm.Attachments != null && EmailVm.Attachments.Count > 0)
        //    //{
        //    //    foreach (var Attachment in EmailVm.Attachments)
        //    //        message.Attachments.Add(Attachment);
        //    //}
        //    //var smtpClient = new SmtpClient
        //    //{
        //    //    Host = _emailSettings.Host,
        //    //    Port = _emailSettings.Port,
        //    //    EnableSsl = true,
        //    //    UseDefaultCredentials = false,
        //    //    DeliveryMethod = SmtpDeliveryMethod.Network,
        //    //    Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password)
        //    //};
        //    //smtpClient.Send(message);

        //    //Uncomment the code below to send email via Send Grid API
        //    var client = new SendGridClient(_emailSettings.ApiKey);
        //    var from = new EmailAddress(_emailSettings.FromAddress, _emailSettings.DisplayName);
        //    var subject = EmailVm.Subject;
        //    var to = new EmailAddress(EmailVm.ToAddresses[0]);
        //    var plainTextContent = "";
        //    var htmlContent = EmailVm.Body;
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    if (EmailVm.SendGridAttachments != null && EmailVm.SendGridAttachments.Count > 0)
        //        msg.AddAttachments(EmailVm.SendGridAttachments);
        //    var response = await client.SendEmailAsync(msg);
        //    if (response.StatusCode == HttpStatusCode.Accepted)
        //    {
        //        //Mail sent successfully
        //    }
        //}
    }
}
