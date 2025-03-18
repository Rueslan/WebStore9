﻿using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using WebStore9.Interfaces;

namespace WebStore9.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTests
    {
        private readonly WebApplicationFactory<Program> _host = new();

        [TestMethod]
        public async Task GetValuesIntegrityTest()
        {
            var client = _host.CreateClient();

            var response = await client.GetAsync(WebAPIAddresses.Values);

            response.EnsureSuccessStatusCode();

            var values = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();

            //var parser = new HtmlParser();
            //var html = parser.ParseDocument();
        }
    }
}
