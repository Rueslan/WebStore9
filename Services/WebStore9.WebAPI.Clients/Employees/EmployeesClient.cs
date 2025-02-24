using System.Net.Http.Json;
using WebStore9.Interfaces.Services;
using WebStore9.WebAPI.Clients.Base;
using WebStore9Domain.Entities;

namespace WebStore9.WebAPI.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(HttpClient client) : base(client, "api/employees") { }

        public IEnumerable<Employee> GetAll()
        {
            var employees = Get<IEnumerable<Employee>>(Address);
            return employees;
        }

        public Employee? GetById(int id)
        {
            var result = Get<Employee?>($"{Address}/{id}");
            return result;
        }

        public int Add(Employee employee)
        {
            var response = Post(Address, employee);
            var addedEmployee = response.Content.ReadFromJsonAsync<Employee>();
            var id = addedEmployee.Id;
            return id;
        }

        public void Update(Employee employee)
        {
            Put(Address, employee);
        }

        public bool Delete(int id)
        {
            var response = Delete($"{Address}/{id}");
            var success = response.IsSuccessStatusCode;
            return success;
        }
    }
}
