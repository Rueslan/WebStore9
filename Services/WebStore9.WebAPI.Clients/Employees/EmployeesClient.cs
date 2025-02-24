using System.Net.Http.Json;
using WebStore9.Interfaces.Services;
using WebStore9.WebAPI.Clients.Base;
using WebStore9Domain.Entities;

namespace WebStore9.WebAPI.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(HttpClient client) : base(client, "api/employees") { }

        public Task<IEnumerable<Employee>> GetAll()
        {
            var employees = GetAsync<IEnumerable<Employee>>(Address);
            return employees;
        }

        public Task<Employee?> GetByIdAsync(int id)
        {
            var result = GetAsync<Employee?>($"{Address}/{id}");
            return result;
        }

        public Task<int> Add(Employee employee)
        {
            var response = Post(Address, employee);
            var addedEmployee = response.Content.ReadFromJsonAsync<Employee>();
            var id = addedEmployee.Id;
            return Task.FromResult(id);
        }

        public Task UpdateAsync(Employee employee)
        {
            Put(Address, employee);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var response = Delete($"{Address}/{id}");
            var success = response.IsSuccessStatusCode;
            return Task.FromResult(success);
        }
    }
}
