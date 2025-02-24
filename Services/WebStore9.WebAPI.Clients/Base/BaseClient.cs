using System.Net.Http.Json;

namespace WebStore9.WebAPI.Clients.Base
{
    public abstract class BaseClient
    {
        protected HttpClient HttpClient { get; }

        protected string Address { get; }

        protected BaseClient(HttpClient client, string address)
        {
            HttpClient = client;
            Address = address;
        }

        protected T Get<T>(string url) => GetAsync<T>(url).Result;
        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await HttpClient.GetAsync(url).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>()
                .ConfigureAwait(false);
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            var response = await HttpClient.PostAsJsonAsync(url, item).ConfigureAwait(false);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            var response = await HttpClient.PutAsJsonAsync(url, item).ConfigureAwait(false);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await HttpClient.DeleteAsync(url).ConfigureAwait(false);

            return response;
        }

    }
}
