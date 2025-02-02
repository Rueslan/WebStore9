using Microsoft.AspNetCore.Mvc;
using WebStore9.Data;
using WebStore9.Models;
using WebStore9.Services.Interfaces;
using WebStore9.ViewModels;

namespace WebStore9.Controllers
{
    [Route("Employees/[action]/{id?}")]
    [Route("stuff/[action]/{id?}")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmplyeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData EmplyeesData, ILogger<EmployeesController> Logger)
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

        #region Delete
        public IActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();

            var employee = _EmplyeesData.GetById(id);
            if (employee == null) 
                return NotFound();

            _EmplyeesData.Delete(id);

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Seniority = employee.Seniority,
            });
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmplyeesData.Delete(id);

            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Edit

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new EmployeeViewModel());

            var employee = _EmplyeesData.GetById((int)id);

            if (employee == null) return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Seniority = employee.Seniority,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model.Name == "мат")
                ModelState.AddModelError("", "Низзя!");

            if (!ModelState.IsValid)
                return View(model);

            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.Patronymic,
                Age = model.Age,
                Seniority = model.Seniority,
            };

            if (employee.Id == 0)
                _EmplyeesData.Add(employee);
            else
                _EmplyeesData.Update(employee);

            return RedirectToAction(nameof(Index));            
        }

        #endregion

        public IActionResult Create() => View("Edit", new EmployeeViewModel());
    }
}
