using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.TestAPI;
using WebStore9.WebAPI.Clients.Base;

namespace WebStore9.WebAPI.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {

        public ValuesClient(HttpClient client) : base(client,"api/values")
        {
            
        }

        public IEnumerable<string> GetAll()
        {
            var response = _httpClient.GetAsync(_address).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result;

            return [];
        }

        public int Count()
        {
            var response = _httpClient.GetAsync($"{_address}/count").Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<int>().Result;

            return -1;
        }

        public string GetById(int id)
        {
            var response = _httpClient.GetAsync($"{_address}/{id}").Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<string>().Result;

            return null;
        }

        public void Add(string value)
        {
            var response = _httpClient.PostAsJsonAsync(_address, value).Result;
            response.EnsureSuccessStatusCode();
        }

        public void Edit(int id, string value)
        {
            var response = _httpClient.PutAsJsonAsync($"{_address}/{id}", value).Result;
            response.EnsureSuccessStatusCode();
        }

        public bool Delete(int id)
        {
            var response = _httpClient.DeleteAsync($"{_address}/{id}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
