using Microsoft.AspNetCore.Mvc;
using WebStore9.Models;

namespace WebStore9.Controllers
{
    public class HomeController : Controller
    {

        private static readonly List<Employee> _Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 29, Seniority = 1 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 35, Seniority = 4 },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидр", Patronymic = "Сидорович", Age = 44, Seniority = 6 }
        };
        public IActionResult Index() => View();

        public IActionResult SomeAction(int id) => Content($"First Controller {id}");

        public IActionResult Employees() => View(_Employees);
    }
}
