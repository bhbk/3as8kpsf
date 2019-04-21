using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Bhbk.Lib.Core.Extensions
{
    public static class HttpContentExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string url, T model)
        {
            var data = JsonConvert.SerializeObject(model);
            var content = new StringContent(data);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return client.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient client, string url, T model)
        {
            var data = JsonConvert.SerializeObject(model);
            var content = new StringContent(data);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return client.PutAsync(url, content);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var data = await content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
