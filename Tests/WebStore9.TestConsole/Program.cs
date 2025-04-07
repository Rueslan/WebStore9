using Clients;

namespace WebStore9.TestConsole
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5025") };

            var api = new WebAPIClient("", client);

            var products = await api.EmployeesGET2Async(2);
        }
    }
}
