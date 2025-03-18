using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using WebStore9.Interfaces;

namespace WebStore9.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTests
    {
        private readonly WebApplicationFactory<Program> _host = new TestWebApplicationFactory();

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

        private class TestWebApplicationFactory : WebApplicationFactory<Program>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.UseEnvironment("Test");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(
                    [
                        new KeyValuePair<string, string>("Database", "Sqlite")
                    ]);
                });
            }
        }
    }
}
