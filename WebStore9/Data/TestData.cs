using WebStore9.Models;

namespace WebStore9.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; set; } = new() 
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 29, Seniority = 1 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 35, Seniority = 4 },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидр", Patronymic = "Сидорович", Age = 44, Seniority = 6 }
        };
    }
}
