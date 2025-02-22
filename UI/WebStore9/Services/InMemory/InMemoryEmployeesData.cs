using WebStore9.Data;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities;

namespace WebStore9.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ILogger<InMemoryEmployeesData> _Logger;

        private int _currentMaxId;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
        {
            _Logger = Logger;
            _currentMaxId = TestData.Employees.Max(e => e.Id);
        }

        public Task<int> Add(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            if (TestData.Employees.Any(e => e.Id == employee.Id)) 
                return Task.FromResult(employee.Id);


            employee.Id = ++_currentMaxId;
            TestData.Employees.Add(employee);

            return Task.FromResult(employee.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var db_employee = await GetByIdAsync(id);
            if (db_employee is null) 
                return false;

            TestData.Employees.Remove(db_employee);

            return true;
        }

        public Task<IEnumerable<Employee>> GetAll() => Task.FromResult(TestData.Employees.AsEnumerable());

        public Task<Employee> GetByIdAsync(int id) => Task.FromResult(TestData.Employees.FirstOrDefault(e => e.Id == id));

        public async Task UpdateAsync(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            var db_employee = await GetByIdAsync(employee.Id);
            if (db_employee is null) 
                return;

            db_employee.LastName = employee.LastName;
            db_employee.FirstName = employee.FirstName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;
            db_employee.Seniority = employee.Seniority;
        }
    }
}
