using Application.AppSettings;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SmsService : ISmsService
    {
        private readonly SmsSettings _smsSettings;
        public SmsService(IOptions<SmsSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
        }
        public Task<string> SendSmsAsync(SmsVM SmsVm)
        {
            var ResponseString = SendSms(SmsVm);
            return Task.FromResult(ResponseString);
        }

        public void OnTaskCompleted(object sender, NotifierEventArgs args)
        {
            //var ResponseString = SendSms(args.SmsVm);
            SendSms(args.SmsVm);
        }

        private string SendSms(SmsVM SmsVm)
        {
            var MobileNoStr = SmsVm.MobileNos[0];
            var BaseUrl = $"{_smsSettings.Url}?username={_smsSettings.Username}&password={_smsSettings.Password}&to={MobileNoStr}&from={_smsSettings.SenderId}&text={SmsVm.MessageText}";
            if (SmsVm.MobileNos.Count > 1)
            {
                SmsVm.MobileNos = SmsVm.MobileNos.Select(x => $"91{x}").ToList();
                MobileNoStr = string.Join(",", SmsVm.MobileNos);
                BaseUrl = $"{_smsSettings.Url}?username={_smsSettings.Username}&password={_smsSettings.Password}&to={MobileNoStr}&from={_smsSettings.SenderId}&text={SmsVm.MessageText}&category=bulk";
            }
            var HttpWebRequest = (HttpWebRequest)WebRequest.Create(BaseUrl);
            var HttpWebResponse = (HttpWebResponse)HttpWebRequest.GetResponse();
            var ResponseStreamReader = new StreamReader(HttpWebResponse.GetResponseStream());
            var ResponseString = ResponseStreamReader.ReadToEnd();
            ResponseStreamReader.Close();
            HttpWebResponse.Close();
            return ResponseString;
        }
    }
}
