using Microsoft.AspNetCore.Mvc;
using WebStore9.Data;
using WebStore9.Models;

namespace WebStore9.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index() => View(TestData.Employees);

        public IActionResult Details(int id)
        {
            var employee = TestData.Employees.SingleOrDefault(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        public IActionResult EmployeeDelete(int id)
        {
            TestData.Employees.Remove(TestData.Employees.SingleOrDefault(e => e.Id == id));

            return View("Index", TestData.Employees);
        }

    }
}
