using Newtonsoft.Json;
namespace WebApp.Helpers
{
    public class CustomErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}