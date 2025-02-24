namespace WebStore9.WebAPI.Clients.Base
{
    public abstract class BaseClient
    {
        protected HttpClient _httpClient { get; }

        protected string _address { get; }

        protected BaseClient(HttpClient client, string address)
        {
            _httpClient = client;
            _address = address;
        }
    }
}
