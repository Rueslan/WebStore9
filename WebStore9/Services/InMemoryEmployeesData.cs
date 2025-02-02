using WebStore9.Data;
using WebStore9.Models;
using WebStore9.Services.Interfaces;

namespace WebStore9.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ILogger<InMemoryEmployeesData> _Logger;

        private int _CurrentMaxId;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
        {
            _Logger = Logger;
            _CurrentMaxId = TestData.Employees.Max(e => e.Id);
        }

        public int Add(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            if (TestData.Employees.Contains(employee)) return employee.Id;

            employee.Id = ++_CurrentMaxId;
            TestData.Employees.Add(employee);

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var db_employee = GetById(id);
            if (db_employee == null) return false;

            TestData.Employees.Remove(db_employee);

            return true;
        }

        public IEnumerable<Employee> GetAll() => TestData.Employees;

        public Employee GetById(int id) => TestData.Employees.SingleOrDefault(e => e.Id == id);

        public void Update(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            if (TestData.Employees.Contains(employee)) return;
            
            var db_employee = GetById(employee.Id);
            if (db_employee == null) return;

            db_employee.LastName = employee.LastName;
            db_employee.FirstName = employee.FirstName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;
            db_employee.Seniority = employee.Seniority;
        }
    }
}
