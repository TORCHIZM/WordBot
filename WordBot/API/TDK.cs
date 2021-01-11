using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Torchizm_Bot.Models;

namespace Torchizm_Bot.API
{
    public static class TDK
    {
        public static HttpClient Client = new HttpClient();
        public static string TdkUri = "https://sozluk.gov.tr/gts";

        public static async Task<bool> CheckWord(string word)
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"{TdkUri}?ara={word}");
            var response = await Client.SendAsync(request);
            var message = response.Content.ReadAsStringAsync().Result;

            try
            {
                var error = JsonConvert.DeserializeObject<ErrorModel>(message);
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
