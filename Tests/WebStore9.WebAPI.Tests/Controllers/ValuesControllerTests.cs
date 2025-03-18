using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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
        }
    }
}
