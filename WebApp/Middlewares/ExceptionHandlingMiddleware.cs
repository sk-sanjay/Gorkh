using Application.Helpers;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IEmailService emailService,
            IConfiguration config
            )
        {
            _next = next;
            _emailService = emailService;
            _config = config;
        }
        //public async Task Invoke(HttpContext context)
        //{
        //    try
        //    {
        //        await _next(context).ConfigureAwait(false);
        //    }
        //    catch (Exception exception)
        //    {
        //        //Send email
        //        var EmailVm = new EmailVM
        //        {
        //            ToAddresses = new List<string> { _config["ErrorEmail"] },
        //            Subject = "App Notification",
        //            Body = MessageBuilder.BuildExceptionMessage(context, exception)
        //        };
        //        await _emailService.SendEmailAsync(EmailVm).ConfigureAwait(false);

        //        //Redirect to Error Page
        //        context.Response.Redirect("/Errors/Exception");
        //    }
        //}
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception).ConfigureAwait(false);
            }
        }
        private async Task HandleException(HttpContext context, Exception exception)
        {
            var request = context.Request;
            var response = context.Response;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = exception.Message;
            var description = exception.InnerException?.Message;
            var msgBuilder = new StringBuilder();
            msgBuilder.AppendLine($"<h1>Message: {message}</h1>");
            msgBuilder.AppendLine($"Source: {exception.Source}<hr />");
            msgBuilder.AppendLine($"Request Path: {request.Path}<hr />");
            msgBuilder.AppendLine($"QueryString: {request.QueryString}<hr />");
            if (exception.StackTrace != null)
                msgBuilder.AppendLine(
                    $"Stack Trace: {exception.StackTrace.Replace(Environment.NewLine, "<br />")}<hr />");
            if (exception.InnerException != null)
                msgBuilder.AppendLine($"Inner Exception<hr />{exception.InnerException?.Message.Replace(Environment.NewLine, "<br />")}<hr />");
            await _emailService.SendEmailAsync(_config["ErrorEmail"], "Api Notification", msgBuilder.ToString(), null, null, null, null).ConfigureAwait(false);
            //Write proper response
            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(new CustomErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Description = description
            }.ToString()).ConfigureAwait(false);
        }
    }
}
