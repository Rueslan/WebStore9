﻿using WebStore9Domain.Entities;

namespace WebStore9.Interfaces.Services
{
    public interface IEmployeesData
    {
        Task<IEnumerable<Employee>> GetAll();

        Task<Employee?> GetByIdAsync(int id);

        Task<int> Add(Employee employee);

        Task UpdateAsync(Employee employee);

        Task<bool> DeleteAsync(int id);
    }
}
