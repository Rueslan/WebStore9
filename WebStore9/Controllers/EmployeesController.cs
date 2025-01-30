using Microsoft.AspNetCore.Mvc;
using WebStore9.Data;
using WebStore9.Models;

namespace WebStore9.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEnumerable<Employee> _Employees;

        public EmployeesController() => _Employees = TestData.Employees;

        public IActionResult Index() => View(_Employees);

        //public IActionResult EmployeeDetails(int id) => View(_Employees[id - 1]);
    }
}
