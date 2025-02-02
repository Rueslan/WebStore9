using Microsoft.AspNetCore.Mvc;
using WebStore9.Data;
using WebStore9.Models;
using WebStore9.Services.Interfaces;

namespace WebStore9.Controllers
{
    [Route("Employees/[action]/{id?}")]
    [Route("stuff/[action]/{id?}")]
    public class EmployeesController : Controller
    {
        private readonly IEmplyeesData _EmplyeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmplyeesData EmplyeesData, ILogger<EmployeesController> Logger)
        {
            _EmplyeesData = EmplyeesData;
            _Logger = Logger;
        }

        [Route("~/employees/all")]
        public IActionResult Index() => View(_EmplyeesData.GetAll());

        [Route("~/employees/info-{id}")]
        public IActionResult Details(int id)
        {
            var employee = _EmplyeesData.GetById(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        public IActionResult EmployeeDelete(int id)
        {
            _EmplyeesData.Delete(id);

            return View("Index", TestData.Employees);
        }

    }
}
