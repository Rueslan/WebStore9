﻿using Microsoft.Extensions.Logging;
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


            employee.Id = ++_currentMaxId;
            TestData.Employees.Add(employee);

            _logger.LogInformation("Сотрудник {0} успешно добавлен",employee);

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var db_employee = GetById(id);
            if (db_employee is null)
            {
                _logger.LogWarning("В процессе попытки удаления сотрудник с id:{0} не найден", id);
                return false;
            }

            TestData.Employees.Remove(db_employee);

            _logger.LogInformation("Сотрудник {0} успешно удалён", db_employee);

            return true;
        }

        public IEnumerable<Employee> GetAll() => TestData.Employees.AsEnumerable();

        public Employee? GetById(int id) => TestData.Employees.FirstOrDefault(e => e.Id == id);

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            var db_employee = GetById(employee.Id);
            if (db_employee is null)
                return;

            db_employee.LastName = employee.LastName;
            db_employee.FirstName = employee.FirstName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;
            db_employee.Seniority = employee.Seniority;

            _logger.LogInformation("Сотрудник {0} успешно обновлён", employee);
        }
    }
}
