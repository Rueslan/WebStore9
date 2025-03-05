using Microsoft.Extensions.Logging;
using WebStore9.Interfaces.Services;
using WebStore9.Services.Data;
using WebStore9Domain.Entities;

namespace WebStore9.Services.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ILogger<InMemoryEmployeesData> _logger;

        private int _currentMaxId;

        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> logger)
        {
            _logger = logger;
            _currentMaxId = TestData.Employees.Max(e => e.Id);
        }

        public int Add(Employee employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            if (TestData.Employees.Any(e => e.Id == employee.Id))
                return employee.Id;

            _logger.LogInformation("Добавление нового сотрудника {0}", employee);

            employee.Id = ++_currentMaxId;
            TestData.Employees.Add(employee);

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var dbEmployee = GetById(id);
            if (dbEmployee is null)
            {
                _logger.LogWarning("В процессе попытки удаления сотрудник с id:{0} не найден", id);
                return false;
            }

            TestData.Employees.Remove(dbEmployee);

            _logger.LogInformation("Сотрудник {0} успешно удалён", dbEmployee);

            return true;
        }

        public IEnumerable<Employee> GetAll()
        {
            _logger.LogInformation("Получение всех сотрудников из памяти");
            return TestData.Employees.AsEnumerable();
        }

        public Employee? GetById(int id)
        {
            _logger.LogInformation("Получение сотрудника из памяти по id: {0}", id);
            return TestData.Employees.FirstOrDefault(e => e.Id == id);
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            var dbEmployee = GetById(employee.Id);
            if (dbEmployee is null)
                return;

            _logger.LogInformation("Изменение данных о сотруднике {0} \n на новые {1}", dbEmployee, employee);

            dbEmployee.LastName = employee.LastName;
            dbEmployee.FirstName = employee.FirstName;
            dbEmployee.Patronymic = employee.Patronymic;
            dbEmployee.Age = employee.Age;
            dbEmployee.Seniority = employee.Seniority;
        }
    }
}
