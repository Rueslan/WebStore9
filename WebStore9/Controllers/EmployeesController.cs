using Microsoft.AspNetCore.Mvc;
using WebStore9.Data;
using WebStore9.Models;

namespace WebStore9.Controllers
{
    [Route("Employees/[action]/{id?}")]
    [Route("stuff/[action]/{id?}")]
    public class EmployeesController : Controller
    {
        [Route("~/employees/all")]
        public IActionResult Index() => View(TestData.Employees);

        [Route("~/employees/info-{id}")]
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
