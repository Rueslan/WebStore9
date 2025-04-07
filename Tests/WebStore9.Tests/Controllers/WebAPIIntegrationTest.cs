using System.Net;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebStore9.Interfaces.TestAPI;
using Assert = Xunit.Assert;

namespace WebStore9.Tests.Controllers
{
    [TestClass]
    public class WebAPIIntegrationTest
    {
        private readonly string[] _expectedValues = Enumerable.Range(1, 10).Select(i => $"TestValue - {i}").ToArray();

        private WebApplicationFactory<Program> _host;

        [TestInitialize]
        public void Initialize()
        {
            var valuesServiceMock = new Mock<IValuesService>();
            valuesServiceMock.Setup(x => x.GetAll()).Returns(_expectedValues);


            _host = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_host => _host
                    .ConfigureServices(service => service
                        .AddSingleton(valuesServiceMock.Object)));
        }

        [TestMethod]
        public async Task GetValues()
        {
            var client = _host.CreateClient();

            var response = await client.GetAsync("/WebAPI");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var parser = new HtmlParser();

            var contentStream = await response.Content.ReadAsStreamAsync();
            var html = parser.ParseDocument(contentStream);

            var items = html.QuerySelectorAll(".container table.table tbody tr td:last-child");

            var actualValues = items.Select(i => i.Text());

            Assert.Equal(_expectedValues, actualValues);
        }
    }
}
