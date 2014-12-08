using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Neurio.Client.Entities.Results;
using Newtonsoft.Json;

namespace Neurio.Client.Extensions
{
    public static class HttpClientExtensions
    {

        public static async Task<T> GetAsType<T>(this HttpClient client, string uri)
        {
            return await ConvertToType<T>(await client.GetAsync(uri));
        }

        public static async Task<T> PostAsType<T>(this HttpClient client, string uri, string data, string contentType)
        {
            return await ConvertToType<T>(await client.PostAsync(uri, new StringContent(data, Encoding.UTF8, contentType)));
        }

        private static async Task<T> ConvertToType<T>(HttpResponseMessage message)
        {
            var resultContent = await message.Content.ReadAsStringAsync();
            T typeResult = default(T);
            if (message.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(resultContent))
                {
                    typeResult = JsonConvert.DeserializeObject<T>(resultContent);
                }
                else
                {
                    typeResult = Activator.CreateInstance<T>();
                }
            }
            else
            {
                typeResult = Activator.CreateInstance<T>();
            }
            var baseResult = (typeResult as BaseResult);
            if (baseResult != null)
            {
                baseResult.Message = resultContent;
                baseResult.StatusCode = (int) message.StatusCode;
                baseResult.Success = message.IsSuccessStatusCode;
            }
            return typeResult;
        }

    }
}