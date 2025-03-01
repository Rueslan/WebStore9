using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces;
using WebStore9.Interfaces.TestAPI;
using WebStore9.WebAPI.Clients.Base;

namespace WebStore9.WebAPI.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {

        public ValuesClient(HttpClient client) : base(client, WebAPIAddresses.Values)
        {
            
        }

        public IEnumerable<string> GetAll()
        {
            var response = HttpClient.GetAsync(Address).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result;

            return [];
        }

        public int Count()
        {
            var response = HttpClient.GetAsync($"{Address}/count").Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<int>().Result;

            return -1;
        }

        public string GetById(int id)
        {
            var response = HttpClient.GetAsync($"{Address}/{id}").Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<string>().Result;

            return null;
        }

        public void Add(string value)
        {
            var response = HttpClient.PostAsJsonAsync(Address, value).Result;
            response.EnsureSuccessStatusCode();
        }

        public void Edit(int id, string value)
        {
            var response = HttpClient.PutAsJsonAsync($"{Address}/{id}", value).Result;
            response.EnsureSuccessStatusCode();
        }

        public bool Delete(int id)
        {
            var response = HttpClient.DeleteAsync($"{Address}/{id}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
