using System.Net;
using System.Net.Http.Json;

namespace WebStore9.WebAPI.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected HttpClient HttpClient { get; }

        protected string Address { get; }

        protected BaseClient(HttpClient client, string address)
        {
            HttpClient = client;
            Address = address;
        }

        protected T Get<T>(string url) => GetAsync<T>(url).Result;
        protected async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NoContent) return default;
            return await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.PostAsJsonAsync(url, item, cancellationToken: cancellationToken).ConfigureAwait(false);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.PutAsJsonAsync(url, item, cancellationToken: cancellationToken).ConfigureAwait(false);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.DeleteAsync(url, cancellationToken).ConfigureAwait(false);

            return response;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (disposing)
            {
                
            }
        }
    }
}
