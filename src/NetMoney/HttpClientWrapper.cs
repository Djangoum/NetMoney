namespace NetMoney.Core
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal class HttpClientWrapper : IDisposable
    {
        private static HttpClient client { get; set; }

        static HttpClientWrapper()
            => client = new HttpClient();

        internal static async Task<TResult> Get<TResult>(string endpointUri)
        {
            var result = await client.GetAsync(endpointUri);
            return JsonConvert.DeserializeObject<TResult>(await result.Content.ReadAsStringAsync());
        }

        public void Dispose()
            => client?.Dispose();
    }
}
